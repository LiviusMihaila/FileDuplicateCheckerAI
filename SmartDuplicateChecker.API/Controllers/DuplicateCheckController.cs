using SmartDuplicateChecker.API.DTOs.Requests;
using SmartDuplicateChecker.API.DTOs.Responses;
using SmartDuplicateChecker.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace SmartDuplicateChecker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DuplicateCheckController : ControllerBase
    {
        private readonly IVectorStore _vectorStore;
        private readonly EmbeddingServiceOptions _options;
        public DuplicateCheckController( IVectorStore vectorStore, IOptions<EmbeddingServiceOptions> options)
        {
            _vectorStore = vectorStore;
            _options = options.Value;
        }

        [HttpPost("check")]
        public async Task<ActionResult<List<DuplicateResult>>> CheckDuplicate([FromBody] DuplicateCheckRequest request)
        {
            string fullPath = Path.Combine(_options.TempFolder, request.FileName);
            if (!System.IO.File.Exists(fullPath))
                return NotFound("The specified file does not exist on the server.");

            try
            {
                string extractedText = await ExtractTextFromTika(fullPath);
                float[] vector = await GetEmbeddingFromAI(extractedText);

                var existingVectors = await _vectorStore.LoadAllVectorsAsync();

                List<DuplicateResult> results = new();

                foreach (var existing in existingVectors)
                {
                    double similarity = VectorComparer.CosineSimilarity(vector, existing.Vector);

                    results.Add(new DuplicateResult
                    {
                        DocumentId = existing.DocumentId,
                        Title = existing.Title,
                        Path = "", // set path here if available
                        Similarity = similarity
                    });
                }

                var topResults = results
                    .OrderByDescending(r => r.Similarity)
                    .Take(10)
                    .ToList();

                // Always store the new vector for future comparisons
                int nextId = existingVectors.Count > 0
                    ? existingVectors.Max(v => v.DocumentId) + 1
                    : 1;

                var newItem = new VectorItem
                {
                    DocumentId = nextId,
                    Title = Path.GetFileNameWithoutExtension(request.FileName),
                    Vector = vector
                };

                await _vectorStore.AddVectorAsync(newItem);

                return Ok(topResults);
            }
            catch (Exception ex)
            {
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        private async Task<string> ExtractTextFromTika(string filePath)
        {
            using var client = new HttpClient();
            using var fileStream = System.IO.File.OpenRead(filePath);

            var content = new StreamContent(fileStream);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri(_options.TikaUrl),
                Content = content
            };
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var text = await response.Content.ReadAsStringAsync();
            if (text.Length < 50)
                throw new Exception("The extracted text is too short. The document might be scanned or empty.");

            return text;
        }

        private async Task<float[]> GetEmbeddingFromAI(string text)
        {
            // Optional safety: limit max size of text sent to AI
            if (text.Length > 10000)
                text = text.Substring(0, 10000);

            using var client = new HttpClient();

            var payload = new { text = text };
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_options.AiUrl, content);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonResponse);
            var embeddingArray = jsonDoc.RootElement.GetProperty("embedding").EnumerateArray();

            return embeddingArray.Select(x => x.GetSingle()).ToArray();
        }
    }
}

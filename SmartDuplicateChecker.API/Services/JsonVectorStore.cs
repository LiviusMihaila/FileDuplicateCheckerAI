using System.Text.Json;

namespace SmartDuplicateChecker.API.Services
{
    public class JsonVectorStore : IVectorStore
    {
        private readonly string _filePath;

        public JsonVectorStore(string filePath = "vectors.json")
        {
            _filePath = filePath;
        }

        public async Task<List<VectorItem>> LoadAllVectorsAsync()
        {
            if (!File.Exists(_filePath))
                return new List<VectorItem>();

            var json = await File.ReadAllTextAsync(_filePath);
            var items = JsonSerializer.Deserialize<List<VectorItem>>(json);
            return items ?? new List<VectorItem>();
        }

        public async Task AddVectorAsync(VectorItem item)
        {
            var list = await LoadAllVectorsAsync();
            list.Add(item);

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(list, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}

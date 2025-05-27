namespace SmartDuplicateChecker.API.Services
{
    public class EmbeddingServiceOptions
    {
        public string TikaUrl { get; set; } = string.Empty;
        public string AiUrl { get; set; } = string.Empty;
        public string TempFolder { get; set; } = string.Empty;
    }
}

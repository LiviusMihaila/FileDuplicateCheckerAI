namespace SmartDuplicateChecker.API.DTOs.Responses
{
    public class DuplicateResult
    {
        public int DocumentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public double Similarity { get; set; }
    }
}

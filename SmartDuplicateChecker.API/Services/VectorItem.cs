namespace SmartDuplicateChecker.API.Services
{
    public class VectorItem
    {
        public int DocumentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public float[] Vector { get; set; } = Array.Empty<float>();
    }
}


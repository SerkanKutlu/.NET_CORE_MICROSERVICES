namespace Core.Entity;

public class Document
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime UploadedAt { get; set; }
    public string OriginalFileName { get; set; }
    public string FileName { get; set; }
    public string MimeType { get; set; }
    public string Path { get; set; }
    
}
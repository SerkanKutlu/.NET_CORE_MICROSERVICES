using GenericMongo.Bases;

namespace Core.Entity
{
    public class DocumentEntity  : BaseEntity
    {
        public string UserId { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime ExpireAt { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string Path { get; set; }
    
    }
}
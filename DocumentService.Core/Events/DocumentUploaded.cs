using System.Text;
using Confluent.Kafka;
using System.Text.Json;

namespace Core.Events;

public class DocumentUploaded : ISerializer<DocumentUploaded>, IDeserializer<DocumentUploaded>
{
    public string DocumentId { get; set; }
    public DateTime UploadedAt { get; set; }

    public DocumentUploaded()
    {
        
    }
    public DocumentUploaded(string documentId, DateTime uploadedAt)
    {
        DocumentId = documentId;
        UploadedAt = uploadedAt;
    }

    public byte[] Serialize(DocumentUploaded data, SerializationContext context)
    {
        var serialized = JsonSerializer.Serialize(data);
        return Encoding.UTF8.GetBytes(serialized);
    }

    public DocumentUploaded Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        var stringDto = Encoding.UTF8.GetString(data);
        return JsonSerializer.Deserialize<DocumentUploaded>(stringDto);
    }
}

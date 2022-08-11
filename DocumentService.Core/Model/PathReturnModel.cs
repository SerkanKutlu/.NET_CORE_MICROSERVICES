using System.Data.Common;

namespace Core.Model
{
    public class PathReturnModel
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
    }
}
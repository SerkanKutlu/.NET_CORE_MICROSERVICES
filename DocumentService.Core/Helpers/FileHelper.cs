using Aspose.Words;
using Core.Entity;
using Core.Exceptions;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace Core.Helpers
{
    
    public class FileHelper: IFileHelper
    {

        private readonly IAuthHelper _authHelper;

        public FileHelper(IAuthHelper authHelper)
        {
            _authHelper = authHelper;
        }

        public string GetMimeType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            var mimeType = string.Empty;
            var result = provider.TryGetContentType(filePath,out mimeType);
            if (!result)
                throw new InvalidMimeTypeException();
            return mimeType;
        }

        private void CheckSupportedType(string mimeType)
        {
            var control = false;
            foreach (var supportedType in SupportedMimeTypes.SupportedTypes)
            {
                if (mimeType == supportedType)
                {
                    control = true;
                }
            }

            if (!control)
                throw new InvalidMimeTypeException(mimeType);
        }

        public void CheckSupportedType(IFormFileCollection files)
        {
            foreach (var file in files)
            {
                var mimeType = GetMimeType(file.FileName);
                CheckSupportedType(mimeType);
            }
        }

        public bool CheckIfImage(string mimeType)
        {
            return mimeType.Contains("image");

        }
        public void CovertToPdfAndSave(IFormFile image, string pathToSave)
        {
            var stream = image.OpenReadStream();
            var doc = new Document();
            var builder = new DocumentBuilder(doc);
            builder.InsertImage(stream);
            doc.Save(pathToSave+$".pdf");
        
        }

        public async Task<DocumentEntity> UploadFiles(IFormFile file, HttpContext httpContext)
        {
            var mimeType = GetMimeType(file.FileName);
            var savingFolder = "Documents";
            //Adding timeStamp to file name
            var date = new DateTimeOffset(DateTime.UtcNow);
            var unixTime = date.ToUnixTimeSeconds().ToString();
            //Setting path
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(),
                $"{savingFolder}\\{unixTime}_{file.FileName}");
            var document = new DocumentEntity
            {
                FileName = $"{unixTime}_{file.FileName}",
                Id = Guid.NewGuid().ToString(),
                MimeType = mimeType,
                OriginalFileName = file.FileName,
                UploadedAt = unixTime,
                UserId = _authHelper.GetAuthenticatedId(httpContext),
                Path = pathToSave
            };
            if (CheckIfImage(mimeType))
            {
                //Convert Image to pdf
                CovertToPdfAndSave(file, pathToSave);
                document.Path += ".pdf";
                document.FileName += ".pdf";
            }
            else
            {
                await using var stream = new FileStream(pathToSave, FileMode.Create);
                await file.CopyToAsync(stream);
            }

            return document;
        }
    }
}
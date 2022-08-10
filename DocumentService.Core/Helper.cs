using Aspose.Words;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace Core;

public static class Helper
{

    public static string GetMimeType(this string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        var mimeType = string.Empty;
        var result = provider.TryGetContentType(filePath,out mimeType);
        if (!result)
            throw new InvalidMimeTypeException();
        return mimeType;
    }

    public static void CheckSupportedType(this string mimeType)
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

    public static bool CheckIfImage(this string mimeType)
    {
        return mimeType.Contains("image");

    }
    public static void CovertToPdfAndSave(IFormFile image, string pathToSave)
    {
        var stream = image.OpenReadStream();
        var doc = new Document();
        var builder = new DocumentBuilder(doc);
        builder.InsertImage(stream);
        doc.Save(pathToSave+$".pdf");
        
    }
}
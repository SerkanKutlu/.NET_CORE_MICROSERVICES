using Core.Enums;
using Core.Exceptions;
using Microsoft.AspNetCore.StaticFiles;

namespace Core;

public static class Helper
{

    public static string GetMimeType(this string filePath)
    {
        var provider = new FileExtensionContentTypeProvider();
        provider.Mappings.Remove(".jpg"); 
        provider.Mappings.Remove(".png");
        provider.Mappings.Remove(".pdf");
        provider.Mappings.Add(".jpg", "jpg");
        provider.Mappings.Add(".png", "png");
        provider.Mappings.Add(".pdf", "pdf");
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
}
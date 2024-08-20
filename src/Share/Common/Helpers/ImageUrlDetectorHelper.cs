namespace Share.Common.Helpers;
public class ImageUrlDetectorHelper
{
    public static bool DetectImageOrUrl(string inputString)
    {
        if (IsBase64ToImage(inputString))
            return true;

        if (IsLink(inputString))
            return true;

        return false;
    }

    public static bool IsLink(string inputString)
    {
        try
        {
            Uri uriResult;
            bool isUrl = Uri.TryCreate(inputString, UriKind.Absolute, out uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return isUrl;
        }
        catch
        {
            return false;
        }

    }

    public static bool IsBase64ToImage(string inputString)
    {
        // Validate Base64 image string before upload image in CDN
        try
        {
            var base64 = inputString.FromBase64String();
            if (base64 == null)
                return false;

            var base64ToImage = base64.BytesToImage();
            if (base64ToImage == null)
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }
}

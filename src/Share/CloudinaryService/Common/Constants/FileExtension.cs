namespace KarnelTravel.Share.CloudinaryService.Common.Constants;
public static class FileExtension
{
    public const string EXTENSION_XLS = ".xls";
    public const string EXTENSION_XLSX = ".xlsx";
    public const string EXTENSION_CSV = ".csv";

    public const string EXTENSION_JPG = ".jpg";
    public const string EXTENSION_JPEG = ".jpeg";
    public const string EXTENSION_PNG = ".png";

    public static IList<string> ALLOWED_FILE_EXTENSIONS = new List<string>
    {
        EXTENSION_XLSX,
        EXTENSION_CSV,
        EXTENSION_XLS,
        EXTENSION_JPG,
        EXTENSION_JPEG,
        EXTENSION_PNG
    };

    public static IList<string> ALLOWED_IMAGE_EXTENSIONS = new List<string>
    {
        EXTENSION_JPG,
        EXTENSION_JPEG,
        EXTENSION_PNG
    };
}

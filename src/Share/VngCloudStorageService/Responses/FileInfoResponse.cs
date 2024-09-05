namespace KarnelTravel.Share.VngCloudStorageService.Responses;

public class BaseFileInfoResponse
{
    public string Key { get; set; }
}

public class FileInfoResponse : BaseFileInfoResponse
{
    public DateTime LastModified { get; set; }
    public long Size { get; set; }
}

public class UploadFileInfoResponse : BaseFileInfoResponse
{
    public string BucketName { get; set; }
    public string fileUrl { get; set; }
}

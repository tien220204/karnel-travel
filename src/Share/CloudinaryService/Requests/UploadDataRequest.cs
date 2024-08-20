namespace KarnelTravel.Share.CloudinaryService.Requests;
public class UploadDataRequest
{
    public byte[] Data { get; set; }

    public string TargetName { get; set; }

    public string Extension { get; set; }

    public string FileName { get; set; }
}

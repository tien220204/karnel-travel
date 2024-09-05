namespace KarnelTravel.Share.VngCloudStorageService.Requests;
public class FileInfoRequest
{
	public string ContainerName { get; set; } = null;
	public string KeyName { get; set; }
}
public class TransferFileRequest
{
	public FileInfoRequest SourceFile { get; set; }
	public FileInfoRequest DestinationFile { get; set; }
}

public class ShareFileRequest : FileInfoRequest
{
	public DateTime Expires { get; set; }
}

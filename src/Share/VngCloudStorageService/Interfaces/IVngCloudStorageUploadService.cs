using KarnelTravel.Share.Common.Models;
using KarnelTravel.Share.VngCloudStorageService.Requests;
using KarnelTravel.Share.VngCloudStorageService.Responses;

namespace KarnelTravel.Share.VngCloudStorageService.Interfaces;
public interface IVngCloudStorageUploadService
{
    /// <summary>
    /// Get list file on Vng Storage
    /// </summary>
    /// <param name="">/param>
    /// <returns></returns>
    Task<AppActionResultData<IList<FileInfoResponse>>> GetAllListFilesAsync(string bucketName);

    /// <summary>
    /// Create folder on Vng Storage
    /// </summary>
    /// <param name="">/param>
    /// <returns></returns>
    Task<AppActionResultData<string>> CreateFolderAsync(string containerName);

    /// <summary>
    /// Make public folder on Vng Storage
    /// </summary>
    /// <param name="">/param>
    /// <returns></returns>
    Task<AppActionResultData<string>> MakePublicFolderAsync();

    /// <summary>
    /// Make private folder on Vng Storage
    /// </summary>
    /// <param name="">/param>
    /// <returns></returns>
    Task<AppActionResultData<string>> MakePrivateFolderAsync();

    /// <summary>
    /// Delete folder on Vng Storage
    /// </summary>
    /// <param name="">/param>
    /// <returns></returns>
    Task<AppActionResultData<string>> DeleteFolderAsync(string containerName);

    /// <summary>
    /// Upload file to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>

    Task<AppActionResultData<UploadFileInfoResponse>> UploadFileAsync(UploadFileInfoRequest uploadRequest);

    /// <summary>
    /// Upload multipart file to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<string>> UploadMultipartFileAsync(UploadFileInfoRequest uploadRequest);

    /// <summary>
    /// Upload multiple files to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<IList<string>>> UploadMultipleFilesAsync(List<UploadFileInfoRequest> uploadRequests);

    /// <summary>
    /// Share file to Vng Storage
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<AppActionResultData<string>> ShareFileAsync(ShareFileRequest shareFileRequest);

    /// <summary>
    /// Move file to Vng Storage
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<AppActionResultData<string>> MoveFileAsync(TransferFileRequest copyFileRequest);

    /// <summary>
    /// Copy file to Vng Storage
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<AppActionResultData<string>> CopyFileAsync(TransferFileRequest copyFileRequest);

    /// <summary>
    /// Rename file to Vng Storage
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    Task<AppActionResultData<string>> RenameFileAsync(TransferFileRequest transferFileRequest);

    /// <summary>
    /// Tag file to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<string>> TagFileAsync(FileInfoRequest fileInfoRequest);

    /// <summary>
    /// Metadata file to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<MetadataFileResponse>> MetadataFileAsync(FileInfoRequest fileInfoRequest);

    // <summary>
    /// Download file to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<string>> DownloadFileAsync(FileInfoRequest fileInfoRequest);

    // <summary>
    /// Delete file to Vng Storage
    /// </summary>
    /// <param name="">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<string>> DeleteFileAsync(FileInfoRequest fileInfoRequest);
}

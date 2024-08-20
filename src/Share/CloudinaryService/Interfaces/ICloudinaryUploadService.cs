using KarnelTravel.Share.Common.Models;
using KarnelTravel.Share.CloudinaryService.Reponses;
using KarnelTravel.Share.CloudinaryService.Requests;

namespace KarnelTravel.Share.CloudinaryService.Interfaces;
public interface ICloudinaryUploadService
{
    /// <summary>
    /// Upload file to Cloudinary Storage
    /// </summary>
    /// <param name="uploadData">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<string>> UploadImageAsync(UploadDataRequest uploadData);


    /// <summary>
    /// Upload file to Cloudinary Storage
    /// </summary>
    /// <param name="uploadData">data be uploaded</param>
    /// <returns></returns>
    Task<AppActionResultData<string>> UploadThumbnailImageAsync(UploadDataThumbnailRequest uploadData);

    /// <summary>
    /// Upload file to Cloudinary Storage
    /// </summary>
    /// <param name="uploadData"></param>
    /// <returns></returns>
    Task<AppActionResultData<UploadFileResponse>> UploadFileAsync(UploadFileRequest uploadData);

    /// <summary>
    /// Upload file to Cloudinary Storage
    /// </summary>
    /// <param name="uploadData"></param>
    /// <returns></returns>
    Task<AppActionResultData<string>> DeleteFileAsync(string publicId);

    /// <summary>
    /// Upload image and thumbnail apply moderation asynchronously
    /// </summary>
    /// <param name="uploadData"></param>
    /// <returns></returns>
    Task<AppActionResultData<ImageAndThumbnailResponse>> UploadImageAndThumbnailApplyModerationAsync(UploadDataThumbnailRequest uploadData);
}

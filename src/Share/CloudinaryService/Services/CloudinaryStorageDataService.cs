using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using KarnelTravel.Share.Common.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using KarnelTravel.Share.CloudinaryService.Common.Constants;
using KarnelTravel.Share.CloudinaryService.Interfaces;
using KarnelTravel.Share.CloudinaryService.Reponses;
using KarnelTravel.Share.CloudinaryService.Requests;
using KarnelTravel.Share.CloudinaryService.Settings;

namespace KarnelTravel.Share.CloudinaryService.Services;
public class CloudinaryStorageDataService : ICloudinaryUploadService
{
    private readonly CloudinaryOAuthApiSettings _cloudinaryOAuthApiSettings;
    private readonly ILogger<CloudinaryStorageDataService> _logger;
    private readonly Cloudinary _cloudinary;

    public CloudinaryStorageDataService(
        ILogger<CloudinaryStorageDataService> logger,
        IOptions<CloudinaryOAuthApiSettings> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cloudinaryOAuthApiSettings = options.Value ?? throw new ArgumentNullException(nameof(CloudinaryOAuthApiSettings));
        _cloudinary = new Cloudinary(new Account
        {
            ApiKey = _cloudinaryOAuthApiSettings.ApiKey,
            ApiSecret = _cloudinaryOAuthApiSettings.ApiSecret,
            Cloud = _cloudinaryOAuthApiSettings.Cloud
        });
    }

    public async Task<AppActionResultData<UploadFileResponse>> UploadFileAsync(UploadFileRequest uploadData)
    {
        var result = new AppActionResultData<UploadFileResponse>();

        if (!FileExtension.ALLOWED_FILE_EXTENSIONS.Contains(uploadData.Extension))
        {
            _logger.LogError("File invalid file extension : {extension}", uploadData.Extension);
            return result.BuildError($"{uploadData.FileName} không hợp lệ. Yêu cầu dữ liệu phải là file theo định dạng đã quy định. Xin kiểm tra lại.");
        }
        _cloudinary.Api.ApiBaseAddress = "http://api.cloudinary.com";

        MemoryStream file = new MemoryStream(uploadData.Data);

        var uploadParams = new RawUploadParams()
        {
            File = new FileDescription(uploadData.FileName, file),
            Folder = uploadData.TargetName,
            FilenameOverride = uploadData.FileName,
            UseFilename = true,
            UniqueFilename = false
        };

        var uploadResult = await _cloudinary.UploadLargeAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            _logger.LogError("Upload file failded: {reason}", uploadResult.Error.Message);
            return result.BuildError($"Upload faild with message: {uploadResult.Error.Message}");
        }
        return result.BuildResult(new UploadFileResponse
        {
            Url = uploadResult.SecureUrl.ToString(),
            PublicId = uploadResult.PublicId,
            UploadTime = uploadResult.CreatedAt
        });
    }

    public async Task<AppActionResultData<string>> UploadImageAsync(UploadDataRequest uploadData)
    {
        var result = new AppActionResultData<string>();

        var buildUrlResult = await ValidateUploadImageAsync(uploadData);
        if (!buildUrlResult.IsSuccess)
        {
            return result.BuildError(buildUrlResult.Detail);
        }

        var uploadLink = _cloudinary.Api.UrlImgUp.Secure(true).BuildUrl(buildUrlResult.Data);

        return result.BuildResult(uploadLink);
    }

    public async Task<AppActionResultData<string>> UploadThumbnailImageAsync(UploadDataThumbnailRequest uploadData)
    {
        var result = new AppActionResultData<string>();
        var buildUrlResult = await ValidateUploadImageAsync(uploadData);
        if (!buildUrlResult.IsSuccess)
        {
            return result.BuildError(buildUrlResult.Detail);
        }
        var uploadLink = _cloudinary.Api.UrlImgUp.Secure(true).Transform(new Transformation().Width(uploadData.Width).Height(uploadData.Height).Crop("scale")).BuildUrl(buildUrlResult.Data);
        return result.BuildResult(uploadLink);
    }

    public async Task<AppActionResultData<string>> DeleteFileAsync(string publicId)
    {
        var result = new AppActionResultData<string>();

        try
        {
            DeletionParams deletionParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var deleteResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deleteResult.Result == "ok")
                return result.BuildResult(deleteResult.Result);
            else
                return result.BuildError(deleteResult.Result);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    /// <summary>
    /// Upload image and thumbnail apply moderation asynchronously
    /// </summary>
    /// <param name="uploadData"></param>
    /// <returns></returns>
    public async Task<AppActionResultData<ImageAndThumbnailResponse>> UploadImageAndThumbnailApplyModerationAsync(UploadDataThumbnailRequest uploadData)
    {
        var result = new AppActionResultData<ImageAndThumbnailResponse>();

        if (!FileExtension.ALLOWED_IMAGE_EXTENSIONS.Contains(uploadData.Extension))
        {
            _logger.LogError("File invalid image extension : {extension}", uploadData.Extension);
            return result.BuildError($"{uploadData.FileName} File invalid image extension.");
        }

        if (_cloudinaryOAuthApiSettings.ApiKey == null || _cloudinaryOAuthApiSettings.Cloud == null || _cloudinaryOAuthApiSettings.ApiSecret == null)
        {
            return result.BuildError($"null");
        }

        var publicId = Guid.NewGuid().ToString("N");
        _cloudinary.Api.ApiBaseAddress = "http://api.cloudinary.com";
        MemoryStream file = new MemoryStream(uploadData.Data);
        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(uploadData.FileName, file),
            PublicId = publicId,
            Folder = uploadData.TargetName,
            Moderation = _cloudinaryOAuthApiSettings.Moderation
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        if (uploadResult.Error != null)
        {
            _logger.LogError("Upload image failded: {reason}", uploadResult.Error.Message);
            return result.BuildError($"Upload faild with message: {uploadResult.Error.Message}");
        }

        var moderations = uploadResult.Moderation;
        var isRejected = moderations?.Any(x => x.Status == ModerationStatus.Rejected) ?? false;
        if (isRejected)
        {
            _logger.LogError("Suspect sensitive images");
            return result.BuildResult(new ImageAndThumbnailResponse
            {
                PublicId = publicId,
                ImageUrl = string.Empty,
                ThumbnailUrl = string.Empty,
                IsRejected = true,
                Moderation = moderations ?? new System.Collections.Generic.List<Moderation>()
            });
        }

        var imageUrl = uploadResult.SecureUrl.ToString();
        var thumbnailUrl = _cloudinary.Api.UrlImgUp.Secure(true).Transform(new Transformation().Width(uploadData.Width).Height(uploadData.Height).Crop("scale")).BuildUrl(imageUrl);

        return result.BuildResult(new ImageAndThumbnailResponse
        {
            PublicId = publicId,
            ImageUrl = imageUrl,
            ThumbnailUrl = thumbnailUrl,
            IsRejected = false
        });
    }

    #region Private Method
    private async Task<AppActionResultData<string>> ValidateUploadImageAsync(UploadDataRequest uploadData)
    {
        var result = new AppActionResultData<string>();

        if (!FileExtension.ALLOWED_IMAGE_EXTENSIONS.Contains(uploadData.Extension))
        {
            _logger.LogError("File invalid image extension : {extension}", uploadData.Extension);
            return result.BuildError($"{uploadData.FileName} không hợp lệ. Yêu cầu dữ liệu phải là hình ảnh theo định dạng đã quy định. Xin kiểm tra lại.");
        }

        if (_cloudinaryOAuthApiSettings.ApiKey == null || _cloudinaryOAuthApiSettings.Cloud == null || _cloudinaryOAuthApiSettings.ApiSecret == null)
        {
            return result.BuildError($"null");
        }

        _cloudinary.Api.ApiBaseAddress = "http://api.cloudinary.com";
        var publicId = Guid.NewGuid().ToString("N");
        var buildUrl = uploadData.TargetName + "/" + publicId + uploadData.Extension.ToLower();

        MemoryStream file = new MemoryStream(uploadData.Data);

        var uploadParams = new ImageUploadParams()
        {
            File = new FileDescription(uploadData.FileName, file),
            PublicId = publicId,
            Folder = uploadData.TargetName
        };

        _ = await _cloudinary.UploadAsync(uploadParams);
        return result.BuildResult(buildUrl);
    }

    #endregion  Private Method
}

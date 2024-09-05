using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using KarnelTravel.Share.Common.Extensions;
using KarnelTravel.Share.Common.Models;
using KarnelTravel.Share.VngCloudStorageService.Common.Contants;
using KarnelTravel.Share.VngCloudStorageService.Interfaces;
using KarnelTravel.Share.VngCloudStorageService.Requests;
using KarnelTravel.Share.VngCloudStorageService.Responses;
using KarnelTravel.Share.VngCloudStorageService.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace KarnelTravel.Share.VngCloudStorageService.Services;
public class VngCloudStorageUploadDataService : IVngCloudStorageUploadService
{
    private readonly VngCloudStorageOAuthApiSettings _vngCloudStorageOAuthApiSettings;
    private readonly ILogger<VngCloudStorageUploadDataService> _logger;
    private readonly AmazonS3Client _amazonS3Client;

    public VngCloudStorageUploadDataService(ILogger<VngCloudStorageUploadDataService> logger,
        IOptions<VngCloudStorageOAuthApiSettings> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _vngCloudStorageOAuthApiSettings = options.Value ?? throw new ArgumentNullException(nameof(VngCloudStorageOAuthApiSettings));

        _amazonS3Client = new AmazonS3Client(
            _vngCloudStorageOAuthApiSettings.AccessKey,
            _vngCloudStorageOAuthApiSettings.SecretKey,
            new AmazonS3Config
            {
                ServiceURL = _vngCloudStorageOAuthApiSettings.ServiceURL,
                ForcePathStyle = _vngCloudStorageOAuthApiSettings.ForcePathStyle,
                AuthenticationRegion = _vngCloudStorageOAuthApiSettings.AuthenticationRegion
            });
    }

    public async Task<AppActionResultData<string>> CopyFileAsync(TransferFileRequest transferFileRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = transferFileRequest.SourceFile.ContainerName.IsNotNullNorEmpty() ?
                    transferFileRequest.SourceFile.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                SourceKey = transferFileRequest.SourceFile.KeyName,

                DestinationBucket = transferFileRequest.DestinationFile.ContainerName.IsNotNullNorEmpty() ?
                    transferFileRequest.DestinationFile.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                DestinationKey = transferFileRequest.DestinationFile.KeyName
            };
            var response = await _amazonS3Client.CopyObjectAsync(request);
            return result.BuildResult(response.ResponseMetadata.RequestId);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> CreateFolderAsync(string containerName)
    {
        var result = new AppActionResultData<string>();
        try
        {
            var request = new PutBucketRequest
            {
                BucketName = containerName,
                UseClientRegion = true,
            };

            var response = await _amazonS3Client.PutBucketAsync(request);
            return result.BuildResult(response.ResponseMetadata.RequestId);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> DeleteFileAsync(FileInfoRequest fileInfoRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {           

            try
            {
                await _amazonS3Client.GetObjectAsync(_vngCloudStorageOAuthApiSettings.BucketName, fileInfoRequest.KeyName);
            }
            catch (AmazonS3Exception s3Ex) when (s3Ex.ErrorCode == "404")
            {
                // Tệp không tồn tại
                return result.BuildError($"File with key {fileInfoRequest.KeyName} does not exist.");
            }
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                Key = fileInfoRequest.KeyName,
            };

            var response = await _amazonS3Client.DeleteObjectAsync(deleteObjectRequest);

            return result.BuildResult(response.ResponseMetadata.RequestId);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    public async Task<AppActionResultData<string>> DeleteFolderAsync(string containerName)
    {
        var result = new AppActionResultData<string>();
        try
        {
            var request = new DeleteBucketRequest
            {
                BucketName = containerName,
            };
            var response = await _amazonS3Client.DeleteBucketAsync(request);
            return result.BuildResult(response.ResponseMetadata.RequestId);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> DownloadFileAsync(FileInfoRequest fileInfoRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {
            TransferUtility transferUtil = new TransferUtility(_amazonS3Client);
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", fileInfoRequest.KeyName).Replace(@"\", @"\\");
            await transferUtil.DownloadAsync(new TransferUtilityDownloadRequest
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                Key = fileInfoRequest.KeyName,
                FilePath = filePath,
            });
            return result.BuildResult(filePath);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    public async Task<AppActionResultData<IList<FileInfoResponse>>> GetAllListFilesAsync(string bucketName)
    {
        var result = new AppActionResultData<IList<FileInfoResponse>>();

        try
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
            };

            IList<FileInfoResponse> files = new List<FileInfoResponse>();

            ListObjectsV2Response response;
            do
            {
                response = await _amazonS3Client.ListObjectsV2Async(request);
                response.S3Objects
                    .ForEach(obj => files.Add(new FileInfoResponse
                    {
                        Key = obj.Key,
                        LastModified = obj.LastModified,
                        Size = obj.Size
                    })
                    );
                request.ContinuationToken = response.NextContinuationToken;
            }
            while (response.IsTruncated);
            return result.BuildResult(files);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }


    }

    public async Task<AppActionResultData<string>> MakePrivateFolderAsync()
    {
        var result = new AppActionResultData<string>();

        try
        {
            var putBucketRequest = new PutBucketRequest()
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                CannedACL = S3CannedACL.Private,
            };
            PutBucketResponse putBucketResponse = await _amazonS3Client.PutBucketAsync(putBucketRequest);

            return result.BuildResult(putBucketRequest.BucketName);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> MakePublicFolderAsync()
    {
        var result = new AppActionResultData<string>();

        try
        {
            var putBucketRequest = new PutBucketRequest()
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                CannedACL = S3CannedACL.PublicRead,
            };
            PutBucketResponse putBucketResponse = await _amazonS3Client.PutBucketAsync(putBucketRequest);

            return result.BuildResult(putBucketRequest.BucketName);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    public async Task<AppActionResultData<MetadataFileResponse>> MetadataFileAsync(FileInfoRequest fileInfoRequest)
    {
        var result = new AppActionResultData<MetadataFileResponse>();
        try
        {
            GetObjectMetadataResponse metadataResponse = await _amazonS3Client.GetObjectMetadataAsync(_vngCloudStorageOAuthApiSettings.BucketName, fileInfoRequest.KeyName);
            var response = new MetadataFileResponse
            {
                CacheControl = metadataResponse.Headers.CacheControl,
                ContentDisposition = metadataResponse.Headers.ContentDisposition,
                ContentEncoding = metadataResponse.Headers.ContentEncoding,
                ContentType = metadataResponse.Headers.ContentType,
                Expires = metadataResponse.Expires,
                XRobotsTag = metadataResponse.Metadata["x-robots-tag"]
            };
            return result.BuildResult(response);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> MoveFileAsync(TransferFileRequest transferFileRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = transferFileRequest.SourceFile.ContainerName.IsNotNullNorEmpty() ?
                    transferFileRequest.SourceFile.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                SourceKey = transferFileRequest.SourceFile.KeyName,

                DestinationBucket = transferFileRequest.DestinationFile.ContainerName.IsNotNullNorEmpty() ?
                    transferFileRequest.DestinationFile.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                DestinationKey = transferFileRequest.DestinationFile.KeyName
            };
            await _amazonS3Client.CopyObjectAsync(request);

            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = request.SourceBucket,
                Key = request.SourceKey,
            };

            var response = await _amazonS3Client.DeleteObjectAsync(deleteObjectRequest);

            return result.BuildResult(response.ResponseMetadata.RequestId);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> RenameFileAsync(TransferFileRequest transferFileRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {
            var request = new CopyObjectRequest
            {
                SourceBucket = transferFileRequest.SourceFile.ContainerName.IsNotNullNorEmpty() ?
                    transferFileRequest.SourceFile.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                SourceKey = transferFileRequest.SourceFile.KeyName,

                DestinationBucket = transferFileRequest.DestinationFile.ContainerName.IsNotNullNorEmpty() ?
                    transferFileRequest.DestinationFile.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                DestinationKey = transferFileRequest.DestinationFile.KeyName
            };
            await _amazonS3Client.CopyObjectAsync(request);

            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = request.SourceBucket,
                Key = request.SourceKey,
            };

            var response = await _amazonS3Client.DeleteObjectAsync(deleteObjectRequest);

            return result.BuildResult(response.ResponseMetadata.RequestId);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }

    public async Task<AppActionResultData<string>> ShareFileAsync(ShareFileRequest shareFileRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {
            var request = new GetPreSignedUrlRequest()
            {
                BucketName = shareFileRequest.ContainerName.IsNotNullNorEmpty() ?
                    shareFileRequest.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                Key = shareFileRequest.KeyName,
                Expires = shareFileRequest.Expires,
            };
            var urlString = await _amazonS3Client.GetPreSignedURLAsync(request);

            return result.BuildResult(urlString);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    public async Task<AppActionResultData<string>> TagFileAsync(FileInfoRequest fileInfoRequest)
    {
        var result = new AppActionResultData<string>();
        try
        {
            GetObjectMetadataResponse response = await _amazonS3Client.GetObjectMetadataAsync(
                fileInfoRequest.ContainerName.IsNotNullNorEmpty() ?
                    fileInfoRequest.ContainerName :
                    _vngCloudStorageOAuthApiSettings.BucketName,
                fileInfoRequest.KeyName);

            var tags = response.Metadata["tags"];
            return result.BuildResult(tags);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    public async Task<AppActionResultData<UploadFileInfoResponse>> UploadFileAsync(UploadFileInfoRequest uploadRequest)
    {
        var result = new AppActionResultData<UploadFileInfoResponse>();

        try
        {
            using var stream = new MemoryStream();
            await uploadRequest.File.CopyToAsync(stream);
            stream.Position = 0;             

            string filename = $"{Path.GetFileNameWithoutExtension(uploadRequest.File.FileName)}_{Guid.NewGuid().ToString()}{Path.GetExtension(uploadRequest.File.FileName)}";

            var objectRrequest = new PutObjectRequest
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                Key = $"{uploadRequest.Key}/{filename}",
                InputStream = stream,
                UseChunkEncoding = uploadRequest.UseChunkEncoding,
            };

            var response = await _amazonS3Client.PutObjectAsync(objectRrequest);

            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = objectRrequest.BucketName,
                Key = objectRrequest.Key,
                Expires = DateTime.UtcNow.AddSeconds(DateTimeConstant.DEFAULT_SECONDS),
            };
            string url = _amazonS3Client.GetPreSignedURL(urlRequest);

            return result.BuildResult(new UploadFileInfoResponse { BucketName = urlRequest .BucketName,Key = urlRequest .Key, fileUrl=url});
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }

    }

    public async Task<AppActionResultData<string>> UploadMultipartFileAsync(UploadFileInfoRequest uploadRequest)
    {
        var result = new AppActionResultData<string>();

        try
        {
            using var stream = new MemoryStream();
            await uploadRequest.File.CopyToAsync(stream);
            stream.Position = 0;

            string url = "";

            TransferUtility transferUtil = new TransferUtility(_amazonS3Client);
            await transferUtil.UploadAsync(new TransferUtilityUploadRequest
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                Key = uploadRequest.Key,
                InputStream = stream,
                PartSize = 1000 * 1024 * 1024,
                DisablePayloadSigning = true,
            });

            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                Key = uploadRequest.Key,
                Expires = DateTime.UtcNow.AddSeconds(DateTimeConstant.DEFAULT_SECONDS),
            };
            url = _amazonS3Client.GetPreSignedURL(urlRequest);

            return result.BuildResult(url);
        }
        catch (AmazonS3Exception s3Ex)
        {
            return result.BuildError(s3Ex.Message);
        }
    }

    public async Task<AppActionResultData<IList<string>>> UploadMultipleFilesAsync(List<UploadFileInfoRequest> uploadRequests)
    {
        var result = new AppActionResultData<IList<string>>();

        try
        {
            
            IList<string> urls = new List<string>();
            foreach (var uploadRequest in uploadRequests)
            {
                using var stream = new MemoryStream();
                await uploadRequest.File.CopyToAsync(stream);
                stream.Position = 0;

                string filename = $"{Path.GetFileNameWithoutExtension(uploadRequest.File.FileName)}_{Guid.NewGuid().ToString()}{Path.GetExtension(uploadRequest.File.FileName)}";
               
                var objectRrequest = new PutObjectRequest
                {
                    BucketName = _vngCloudStorageOAuthApiSettings.BucketName,
                    Key = $"{uploadRequest.Key}/{filename}",
                    InputStream = stream,
                    UseChunkEncoding = uploadRequest.UseChunkEncoding,
                };

                var response = await _amazonS3Client.PutObjectAsync(objectRrequest);

                var urlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = objectRrequest.BucketName,
                    Key = objectRrequest.Key,
                    Expires = DateTime.UtcNow.AddSeconds(DateTimeConstant.DEFAULT_SECONDS),
                };
                string url = _amazonS3Client.GetPreSignedURL(urlRequest);
                urls.Add(url);
            }

            return result.BuildResult(urls);
        }
        catch (Exception ex)
        {
            return result.BuildError(ex.Message);
        }
    }
}

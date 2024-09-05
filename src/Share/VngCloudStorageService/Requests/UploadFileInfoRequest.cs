using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KarnelTravel.Share.VngCloudStorageService.Requests;
public class UploadFileInfoRequest
{
    public IFormFile File { get; set; }
    public string Key { get; set; }
    public bool UseChunkEncoding { get; set; } = false;
}

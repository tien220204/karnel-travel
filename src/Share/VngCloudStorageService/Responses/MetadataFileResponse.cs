using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Share.VngCloudStorageService.Responses;
public class MetadataFileResponse
{
    public string CacheControl { get; set; }
    public string ContentDisposition { get; set; }
    public string ContentEncoding { get; set; }
    public string ContentType { get; set; }
    public DateTime Expires { get; set; }
    public string XRobotsTag { get; set; }
}

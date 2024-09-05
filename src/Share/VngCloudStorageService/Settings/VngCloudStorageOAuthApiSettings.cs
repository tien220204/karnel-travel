using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarnelTravel.Share.VngCloudStorageService.Settings;
public class VngCloudStorageOAuthApiSettings
{
    public string ServiceURL { get; set; }
    public bool ForcePathStyle { get; set; }
    public string AuthenticationRegion { get; set; }
    public string BucketName { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}

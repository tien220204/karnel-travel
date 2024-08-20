using System.ComponentModel.DataAnnotations;
using KarnelTravel.Share.Common.Enums;

namespace KarnelTravel.Share.Common.Models;
public class AppMessage
{
    [Required]
    [StringLength(10000)]
    public string Content { get; set; }

    public AppMessageType Type { get; set; }
}

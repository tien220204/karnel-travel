using CloudinaryDotNet.Actions;

namespace KarnelTravel.Share.CloudinaryService.Reponses;
public class ImageAndThumbnailResponse
{
    public string? PublicId { get; set; }
    public string? ImageUrl { get; set; }
    public string? ThumbnailUrl { get; set; }
    public bool IsRejected { get; set; }
    public IList<Moderation> Moderation { get; set; } = new List<Moderation>();
}

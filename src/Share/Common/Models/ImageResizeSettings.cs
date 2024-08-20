using System.Drawing.Imaging;

namespace Share.Common.Models;
public class ImageResizeSettings
{
    public uint ResizedWidth { get; set; }

    public uint ResizedHeight { get; set; }

    public bool KeepOriginalRate { get; set; }

    public bool UsingSmallerFactor { get; set; }

    public PixelFormat ImgFormat { get; set; }

    public uint? MinWidth { get; set; }

    public uint? MinHeight { get; set; }

    public uint? MaxWidth { get; set; }

    public uint? MaxHeight { get; set; }

    public ImageResizeSettings()
    {
        KeepOriginalRate = true;
        UsingSmallerFactor = true;
        ImgFormat = PixelFormat.Format24bppRgb;
        MinWidth = null;
        MinHeight = null;
        MaxWidth = null;
        MaxHeight = null;
    }
}

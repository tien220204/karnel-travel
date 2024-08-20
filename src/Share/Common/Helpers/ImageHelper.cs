
using System.Drawing;
using System.Drawing.Imaging;
using Share.Common.Models;

namespace Share.Common.Helpers;
public static class ImageHelper
{
    /// <summary>
    /// Convert bytes to image
    /// </summary>
    /// <param name="imgBytes"></param>
    /// <returns></returns>
    public static Bitmap BytesToImage(this byte[] imgBytes)
    {
        Bitmap result = null;
        if (imgBytes != null)
        {
            MemoryStream stream = new MemoryStream(imgBytes);
            result = Image.FromStream(stream) as Bitmap;
        }

        return result;
    }

    /// <summary>
    /// Resizes an image to difference size
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="resizeSettings"></param>
    /// <returns></returns>
    public static Bitmap ResizeImage(this Bitmap originalImage, ImageResizeSettings resizeSettings)
    {
        Bitmap bitmap = null;
        double num = GetValueInRange(resizeSettings.ResizedWidth, resizeSettings.MinWidth, resizeSettings.MaxWidth);
        double num2 = GetValueInRange(resizeSettings.ResizedHeight, resizeSettings.MinHeight, resizeSettings.MaxHeight);
        if (originalImage != null)
        {
            ImageFormat rawFormat = originalImage.RawFormat;
            double num3 = num / originalImage.Width;
            double num4 = num2 / originalImage.Height;
            if (num3 > 0.0 && num4 > 0.0)
            {
                if (resizeSettings.KeepOriginalRate)
                {
                    double num5 = 1.0;
                    num5 = !(num3 >= num4) ? resizeSettings.UsingSmallerFactor ? num3 : num4 : resizeSettings.UsingSmallerFactor ? num4 : num3;
                    num = originalImage.Width * num5;
                    num2 = originalImage.Height * num5;
                }
            }
            else
            {
                num = originalImage.Width;
                num2 = originalImage.Height;
            }

            int width = (int)Math.Round(num);
            int height = (int)Math.Round(num2);
            
            bitmap = new Bitmap(width, height, resizeSettings.ImgFormat);
            bitmap.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
            Rectangle srcRect = new Rectangle(0, 0, originalImage.Width, originalImage.Height);

            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(originalImage, new Rectangle(0, 0, width, height), srcRect, GraphicsUnit.Pixel);
        }

        return bitmap;
    }

    /// <summary>
    /// Get Thumbnail of image
    /// </summary>
    /// <param name="originalImage"></param>
    /// <param name="resizeSettings"></param>
    /// <returns></returns>
    public static Image RenderImageThumbnail(this Bitmap originalImage, ImageResizeSettings resizeSettings)
    {
        float num = originalImage.Width > resizeSettings.ResizedWidth ? resizeSettings.ResizedWidth / (float)originalImage.Width : 1f;
        float num2 = originalImage.Height > resizeSettings.ResizedHeight ? resizeSettings.ResizedHeight / (float)originalImage.Height : 1f;
        int num3 = 0;
        int num4 = 0;
        if (num > num2)
        {
            num3 = (int)Math.Ceiling(num2 * originalImage.Width);
            num4 = (int)Math.Ceiling(num2 * originalImage.Height);
        }
        else
        {
            num3 = (int)Math.Ceiling(num * originalImage.Width);
            num4 = (int)Math.Ceiling(num * originalImage.Height);
        }

        return originalImage.GetThumbnailImage(num3, num4, () => false, IntPtr.Zero);
    }

    //
    // Summary:
    //     Resizes an image for saving.
    //
    // Parameters:
    //   imageBytes:
    //     image bytes
    //
    //   resizeSettings:
    //     resize settings
    public static byte[] ResizeImage(this byte[] imageBytes, ImageResizeSettings resizeSettings)
    {
        using Bitmap originalImage = imageBytes.BytesToImage();
        Bitmap img = originalImage.ResizeImage(resizeSettings);
        return img.ImageToBytes(resizeSettings.ImgFormat.ToImageFormat());
    }

    /// <summary>
    /// Convert drawing image to byte array
    /// </summary>
    /// <param name="img"></param>
    /// <param name="imgFormat"></param>
    /// <returns></returns>
    public static byte[] ImageToBytes(this Image img, ImageFormat imgFormat)
    {
        byte[] result = null;
        if (img != null)
        {
            using MemoryStream memoryStream = new MemoryStream();
            img.Save(memoryStream, imgFormat);
            result = memoryStream.ToArray();
        }

        return result;
    }

    /// <summary>
    /// Converts pixel format to image format
    /// </summary>
    /// <param name="pixelFormat"></param>
    /// <returns></returns>
    public static ImageFormat ToImageFormat(this PixelFormat pixelFormat)
    {
        int num;
        switch (pixelFormat)
        {
            case PixelFormat.Format24bppRgb:
                return ImageFormat.Jpeg;
            default:
                num = pixelFormat == PixelFormat.Format64bppPArgb ? 1 : 0;
                break;
            case PixelFormat.Format32bppRgb:
            case PixelFormat.Format48bppRgb:
            case PixelFormat.Format32bppArgb:
            case PixelFormat.Format64bppArgb:
                num = 1;
                break;
        }

        if (num != 0)
        {
            return ImageFormat.Png;
        }

        return ImageFormat.Jpeg;
    }

    /// <summary>
    /// Gets the value in range.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="val">value</param>
    /// <param name="minValue">minimum value</param>
    /// <param name="maxValue">maximum value</param>
    /// <returns></returns>
    private static T GetValueInRange<T>(T val, T? minValue = null, T? maxValue = null)
        where T : struct, IComparable
    {
        var returnVal = val;
        if (minValue.HasValue)
        {
            if (returnVal.CompareTo(minValue.Value) < 0)
            {
                returnVal = minValue.Value;
            }
        }
        if (maxValue.HasValue)
        {
            if (returnVal.CompareTo(maxValue.Value) > 0)
            {
                returnVal = maxValue.Value;
            }
        }
        return returnVal;
    }
}

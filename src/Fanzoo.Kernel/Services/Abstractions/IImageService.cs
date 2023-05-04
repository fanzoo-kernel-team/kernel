namespace Fanzoo.Kernel.Services
{
    public enum ImageFormat
    {
        Png,
        Jpeg,
        Gif,
        Svg,
        Bmp,
        Heif,
        Ico,
        Tiff,
        Webp,
        Wbmp,
        Pkm,
        Ktx,
        Astc,
        Dng
    }

    public interface IImageService
    {
        ValueTask<Stream> ScaleAndCenterImageAsync(Stream image, int targetWidth, int targetHeight, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        ValueTask<byte[]> ScaleAndCenterImageAsync(byte[] image, int targetWidth, int targetHeight, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);
    }
}

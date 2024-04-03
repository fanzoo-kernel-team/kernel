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

        ValueTask<Stream> ShrinkToMaxAsync(Stream image, int maxSize, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        ValueTask<byte[]> ShrinkToMaxAsync(byte[] image, int maxSize, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        Stream OverlayImage(Stream backgroundImage, Stream overlayImage, int x, int y, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        byte[] OverlayImage(byte[] backgroundImage, byte[] overlayImage, int x, int y, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        Stream OverlayCenteredImage(Stream backgroundImage, Stream overlayImage, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        byte[] OverlayCenteredImage(byte[] backgroundImage, byte[] overlayImage, ImageFormat imageFormat = ImageFormat.Png, int quality = 100);

        Stream ConvertImage(Stream image, ImageFormat imageFormat, int quality = 100);

        byte[] ConvertImage(byte[] image, ImageFormat imageFormat, int quality = 100);
    }
}

using SkiaSharp;

namespace Fanzoo.Kernel.Services
{
    public sealed class SkiaImageService : IImageService
    {
        public ValueTask<Stream> ScaleAndCenterImageAsync(Stream image, int targetWidth, int targetHeight, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var originalImage = SKBitmap.Decode(image);

            var scalingFactor = Math.Min((float)targetWidth / originalImage.Width, (float)targetHeight / originalImage.Height);

            var newWidth = (int)(originalImage.Width * scalingFactor);
            var newHeight = (int)(originalImage.Height * scalingFactor);

            var posX = (targetWidth - newWidth) / 2;
            var posY = (targetHeight - newHeight) / 2;

            var scaledImage = new SKBitmap(targetWidth, targetHeight, SKColorType.Rgba8888, SKAlphaType.Premul);

            scaledImage.Erase(SKColors.Transparent);

            using var canvas = new SKCanvas(scaledImage);

            var destRect = new SKRect(posX, posY, posX + newWidth, posY + newHeight);

            using var paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true
            };

            canvas.DrawBitmap(originalImage, destRect, paint);

            return new ValueTask<Stream>(scaledImage.Encode(imageFormat.ToSKEncodedImageFormat(), quality).AsStream());
        }

        public async ValueTask<byte[]> ScaleAndCenterImageAsync(byte[] image, int targetWidth, int targetHeight, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var imageStream = new MemoryStream(image);

            var scaledImageStream = await ScaleAndCenterImageAsync(imageStream, targetWidth, targetHeight, imageFormat, quality);

            return scaledImageStream.ReadAllBytes();
        }
    }

    public static class SkiaImageServiceExtensions
    {
        public static SKEncodedImageFormat ToSKEncodedImageFormat(this ImageFormat imageFormat) => imageFormat switch
        {
            ImageFormat.Png => SKEncodedImageFormat.Png,
            ImageFormat.Jpeg => SKEncodedImageFormat.Jpeg,
            ImageFormat.Gif => SKEncodedImageFormat.Gif,
            ImageFormat.Svg => throw new NotImplementedException(),
            ImageFormat.Bmp => SKEncodedImageFormat.Bmp,
            ImageFormat.Heif => SKEncodedImageFormat.Heif,
            ImageFormat.Ico => SKEncodedImageFormat.Ico,
            ImageFormat.Tiff => throw new NotImplementedException(),
            ImageFormat.Webp => SKEncodedImageFormat.Webp,
            ImageFormat.Wbmp => SKEncodedImageFormat.Wbmp,
            ImageFormat.Pkm => SKEncodedImageFormat.Pkm,
            ImageFormat.Ktx => SKEncodedImageFormat.Ktx,
            ImageFormat.Astc => SKEncodedImageFormat.Astc,
            ImageFormat.Dng => SKEncodedImageFormat.Dng,
            _ => throw new NotImplementedException(),
        };
    }
}

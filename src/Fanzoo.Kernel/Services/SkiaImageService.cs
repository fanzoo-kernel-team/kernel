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

            var scaledImage = ScaleImage(originalImage, targetWidth, targetHeight, newWidth, newHeight, posX, posY);

            return new ValueTask<Stream>(scaledImage.Encode(imageFormat.ToSKEncodedImageFormat(), quality).AsStream());
        }

        public async ValueTask<byte[]> ScaleAndCenterImageAsync(byte[] image, int targetWidth, int targetHeight, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var imageStream = new MemoryStream(image);

            var scaledImageStream = await ScaleAndCenterImageAsync(imageStream, targetWidth, targetHeight, imageFormat, quality);

            return scaledImageStream.ReadAllBytes();
        }

        public ValueTask<Stream> ShrinkToMaxAsync(Stream image, int maxSize, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var originalImage = SKBitmap.Decode(image);

            if (originalImage.Width <= maxSize && originalImage.Height <= maxSize)
            {
                return new ValueTask<Stream>(originalImage.Encode(imageFormat.ToSKEncodedImageFormat(), quality).AsStream());
            }

            var scalingFactor = originalImage.Width > originalImage.Height ? (float)maxSize / originalImage.Width : (float)maxSize / originalImage.Height;

            var targetWidth = (int)(originalImage.Width * scalingFactor);
            var targetHeight = (int)(originalImage.Height * scalingFactor);

            var scaledImage = ScaleImage(originalImage, targetWidth, targetHeight);

            return new ValueTask<Stream>(scaledImage.Encode(imageFormat.ToSKEncodedImageFormat(), quality).AsStream());
        }

        public async ValueTask<byte[]> ShrinkToMaxAsync(byte[] image, int maxSize, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var imageStream = new MemoryStream(image);

            var scaledImageStream = await ShrinkToMaxAsync(imageStream, maxSize, imageFormat, quality);

            return scaledImageStream.ReadAllBytes();
        }

        public Stream OverlayImage(Stream backgroundImage, Stream overlayImage, int x, int y, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            using var backgroundBmp = SKBitmap.Decode(backgroundImage);
            using var overlayBmp = SKBitmap.Decode(overlayImage);

            return OverlayImage(backgroundBmp, overlayBmp, x, y, imageFormat, quality);
        }

        public byte[] OverlayImage(byte[] backgroundImage, byte[] overlayImage, int x, int y, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            using var backgroundImageStream = new MemoryStream(backgroundImage);
            using var overlayImageStream = new MemoryStream(overlayImage);

            var outputImageStream = OverlayImage(backgroundImageStream, overlayImageStream, x, y, imageFormat, quality);

            return outputImageStream.ReadAllBytes();
        }

        public Stream OverlayCenteredImage(Stream backgroundImage, Stream overlayImage, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var backgroundBmp = SKBitmap.Decode(backgroundImage);
            var overlayBmp = SKBitmap.Decode(overlayImage);

            var posX = (backgroundBmp.Width - overlayBmp.Width) / 2;
            var posY = (backgroundBmp.Height - overlayBmp.Height) / 2;

            return OverlayImage(backgroundBmp, overlayBmp, posX, posY, imageFormat, quality);
        }

        public byte[] OverlayCenteredImage(byte[] backgroundImage, byte[] overlayImage, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            using var backgroundImageStream = new MemoryStream(backgroundImage);
            using var overlayImageStream = new MemoryStream(overlayImage);

            var outputImageStream = OverlayCenteredImage(backgroundImageStream, overlayImageStream, imageFormat, quality);

            return outputImageStream.ReadAllBytes();
        }

        public Stream ConvertImage(Stream image, ImageFormat imageFormat, int quality = 100)
        {
            var originalImage = SKBitmap.Decode(image);

            return originalImage.Encode(imageFormat.ToSKEncodedImageFormat(), quality).AsStream();
        }

        public byte[] ConvertImage(byte[] image, ImageFormat imageFormat, int quality = 100)
        {
            var imageStream = new MemoryStream(image);

            var outputImageStream = ConvertImage(imageStream, imageFormat, quality);

            return outputImageStream.ReadAllBytes();
        }

        private static Stream OverlayImage(SKBitmap backgroundBmp, SKBitmap overlayBmp, int x, int y, ImageFormat imageFormat = ImageFormat.Png, int quality = 100)
        {
            var backgroundInfo = new SKImageInfo(backgroundBmp.Width, backgroundBmp.Height);
            using var surface = SKSurface.Create(backgroundInfo);

            var canvas = surface.Canvas;

            canvas.DrawBitmap(backgroundBmp, 0, 0);

            var paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true,
                BlendMode = SKBlendMode.SrcOver,
            };

            canvas.DrawBitmap(overlayBmp, x, y, paint);

            var outputImage = surface.Snapshot();

            return outputImage.Encode(imageFormat.ToSKEncodedImageFormat(), quality).AsStream();
        }

        private static SKBitmap ScaleImage(SKBitmap originalImage, int targetWidth, int targetHeight, int x = 0, int y = 0, SKColorType colorType = SKColorType.Rgba8888, SKAlphaType alphaType = SKAlphaType.Premul) => ScaleImage(originalImage, targetWidth, targetHeight, targetWidth, targetHeight, x, y, colorType, alphaType);

        private static SKBitmap ScaleImage(SKBitmap originalImage, int targetWidth, int targetHeight, int width, int height, int x = 0, int y = 0, SKColorType colorType = SKColorType.Rgba8888, SKAlphaType alphaType = SKAlphaType.Premul)
        {
            var scaledImage = new SKBitmap(targetWidth, targetHeight, colorType, alphaType);

            scaledImage.Erase(SKColors.Transparent);

            using var canvas = new SKCanvas(scaledImage);

            var destRect = new SKRect(x, y, x + width, y + height);

            using var paint = new SKPaint
            {
                FilterQuality = SKFilterQuality.High,
                IsAntialias = true
            };

            canvas.DrawBitmap(originalImage, destRect, paint);

            return scaledImage;
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

using Fanzoo.Kernel.Storage.Blob.Services;

namespace Fanzoo.Kernel.Builder
{
    public sealed class BlobStorageFactoryBuilder
    {
        public BlobStorageFactoryBuilder(WebApplicationBuilder builder)
        {
            WebApplicationBuilder = builder;
        }

        public WebApplicationBuilder WebApplicationBuilder { get; private set; }
    }

    public static class BlobStorageFactoryBuilderExtensions
    {
        public static BlobStorageFactoryBuilder AddAzureBlobStorage(this BlobStorageFactoryBuilder builder)
        {
            builder.WebApplicationBuilder.AddTransient<IBlobStorageService, AzureBlobStorageService>();

            builder.WebApplicationBuilder.AddSetting<AzureBlobStorageSettings>(AzureBlobStorageSettings.SectionName);

            return builder;
        }

        public static BlobStorageFactoryBuilder AddFileBlobStorage(this BlobStorageFactoryBuilder builder)
        {
            builder.WebApplicationBuilder.AddTransient<IBlobStorageService, FileBlobStorageService>();

            builder.WebApplicationBuilder.AddSetting<FileBlobStorageSettings>(FileBlobStorageSettings.SectionName);

            return builder;
        }
    }

    public static partial class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddBlobStorageFactory(this WebApplicationBuilder builder, Action<BlobStorageFactoryBuilder> configuration)
        {
            builder.Services.AddTransient<IBlobStorageServiceFactory, BlobStorageServiceFactory>();

            builder.AddSetting<BlobStorageServiceFactorySettings>(BlobStorageServiceFactorySettings.SectionName);

            var factoryBuilder = new BlobStorageFactoryBuilder(builder);

            configuration.Invoke(factoryBuilder);

            return builder;
        }
    }
}

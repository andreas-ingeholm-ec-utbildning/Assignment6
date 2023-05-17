namespace WebApp.Services;

/// <summary>Manages image uploads.</summary>
public class ImageService
{

    readonly IWebHostEnvironment webHost;

    public ImageService(IWebHostEnvironment webHost) =>
        this.webHost = webHost;

    /// <summary>Uploads an image to the server.</summary>
    /// <returns>The url of the uploaded image. <see langword="null"/> if an error occured.</returns>
    public async Task<string?> UploadImage<TModel>(IFormFile? file, Guid ModelID)
    {

        if (file is null || !file.ContentType.StartsWith("image/"))
            return null;

        try
        {

            var imageUrl = $"/images/{typeof(TModel).Name.ToLower()}/{ModelID}_{file.FileName}";

            var imagePath = $"{webHost.WebRootPath}{imageUrl}";
            using var fs = new FileStream(imagePath, FileMode.Create, FileAccess.Write);

            await file.CopyToAsync(fs);
            return imageUrl;

        }
        catch (Exception)
        {
            return null;
        }

    }

}

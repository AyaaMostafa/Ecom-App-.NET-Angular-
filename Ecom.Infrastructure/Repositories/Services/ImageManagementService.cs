using Ecom.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace Ecom.Infrastructure.Repositories.Services
{
    
    public class ImageManagementService : IImageManagementService
    {

        public readonly IFileProvider fileProvider;
        public ImageManagementService(IFileProvider fileProvider)
        {
            this.fileProvider = fileProvider;
        }
        public async Task<List<string>> AddImagesAsync(IFormFileCollection files, string src)
        {
            var SaveImageSrc = new List<string>();
            var ImageDirectory = Path.Combine("wwwroot", "Images", src);
            if (!Directory.Exists(ImageDirectory))
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            foreach (var item in files)
            {
                if (item.Length > 0)
                {
                    var fileExtension = Path.GetExtension(item.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                    var imageSrc = $"/Images/{src}/{uniqueFileName}";
                    var root = Path.Combine(ImageDirectory, uniqueFileName);

                    using (FileStream stream = new FileStream(root, FileMode.Create))
                    {
                        await item.CopyToAsync(stream);
                    }
                    SaveImageSrc.Add(uniqueFileName);
                }
            }
                return SaveImageSrc;
            }
        
        

        public void DeleteImageAsync(string src)
        {
            var info= fileProvider.GetFileInfo(src);
            var root= info.PhysicalPath;
            File.Delete(root);
        }
    }
}

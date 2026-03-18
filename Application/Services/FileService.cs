using Infrastructure.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment env;

        public FileService(IHostEnvironment env)
        {
            this.env = env;
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName); //.jpg
            string randomFileName = $"{Guid.NewGuid()}{extension}";
            string fullName = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", randomFileName);

            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fs);
            }

            return randomFileName;
        }

        public Task<string> ChangeFileAsync(string oldFileName, IFormFile file)
        {
            string oldFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", oldFileName);

            if (System.IO.File.Exists(oldFilePath))
            {
                string archiveFilePath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", $"archive-{oldFileName}");

                System.IO.File.Move(oldFilePath, archiveFilePath);
            }

            return UploadAsync(file);
        }

        public string GetProductImageUrl(int productId)
        {
            // Logic to determine image URL based on product ID
            // For example, you might construct the URL based on a known path pattern and the product ID
            string imageUrl = $"~/NFTMarketplace/assets/images/product_{productId}.jpg";

            return imageUrl;
        }
    }
}

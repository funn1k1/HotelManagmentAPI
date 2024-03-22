using HotelManagment_API.Services.Interfaces;

namespace HotelManagment_API.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _accessor;

        public ImageService(IWebHostEnvironment hostEnvironment, IHttpContextAccessor accessor)
        {
            _hostEnvironment = hostEnvironment;
            _accessor = accessor;
        }

        public async Task<string?> UploadAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return default;
            }

            var imageDirectory = Path.Combine(_hostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(imageDirectory))
            {
                Directory.CreateDirectory(imageDirectory);
            }

            var imageExtension = Path.GetExtension(imageFile.FileName);
            var imageName = Guid.NewGuid() + imageExtension;
            var imagePath = Path.Combine(imageDirectory, imageName);
            using (var fsStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fsStream);
            }

            var request = _accessor.HttpContext.Request;
            var imageUrl = $"{request.Scheme}://{request.Host}/images/{imageName}";
            return imageUrl;
        }

        public async Task<string?> UpdateAsync(IFormFile? imageFile, string? oldImageUrl)
        {
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                Delete(oldImageUrl);
            }

            return await UploadAsync(imageFile);
        }

        public void Delete(string? imageUrl)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, imageUrl.Substring(imageUrl.IndexOf("images")));
            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }
        }
    }
}

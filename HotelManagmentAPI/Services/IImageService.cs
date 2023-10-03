namespace HotelManagment_API.Services
{
    public interface IImageService
    {
        Task<string?> UploadAsync(IFormFile? formFile);

        Task<string?> UpdateImageAsync(IFormFile? imageFile, string? oldImageUrl);

        void Delete(string? imageUrl);
    }
}

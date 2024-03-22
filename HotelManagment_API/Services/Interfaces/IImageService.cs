namespace HotelManagment_API.Services.Interfaces
{
    public interface IImageService
    {
        Task<string?> UploadAsync(IFormFile? imageFile);

        Task<string?> UpdateAsync(IFormFile? imageFile, string? oldImageUrl);

        void Delete(string? imageUrl);
    }
}

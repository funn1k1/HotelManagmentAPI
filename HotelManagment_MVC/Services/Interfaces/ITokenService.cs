namespace HotelManagment_MVC.Services.Interfaces
{
    public interface ITokenService
    {
        Task<APIResponse<T>> RevokeTokenAsync<T>(string userName);
    }
}

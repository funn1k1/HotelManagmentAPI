namespace HotelManagment_MVC.Services.Interfaces
{
    public interface ITokenService
    {
        Task<APIResponse<T>> RevokeTokenAsync<T>();

        Task<APIResponse<T>> RefreshTokenAsync<T, K>(K entity);
    }
}

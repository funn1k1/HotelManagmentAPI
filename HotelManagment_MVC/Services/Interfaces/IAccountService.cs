namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IAccountService
    {
        Task<APIResponse<T>> Register<T, K>(K entity);

        Task<APIResponse<T>> Login<T, K>(K entity);
    }
}

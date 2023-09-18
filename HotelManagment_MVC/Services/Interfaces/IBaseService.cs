namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IBaseService
    {
        Task<APIResponse<T>> SendAsync<T, K>(APIRequest<K> request);
    }
}

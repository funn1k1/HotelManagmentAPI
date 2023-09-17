namespace HotelManagment_MVC.Services.Interfaces
{
    public interface IBaseService
    {
        Task<APIResponse<string>> SendAsync<T>(APIRequest<T> request);
    }
}

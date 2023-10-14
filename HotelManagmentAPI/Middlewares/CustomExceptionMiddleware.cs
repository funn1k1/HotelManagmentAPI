using Newtonsoft.Json;

namespace HotelManagment_API.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public CustomExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context, IHostEnvironment env)
        {
            try
            {
                await _requestDelegate.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, env);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
        {
            context.Response.ContentType = "application/json";
            if (env.IsDevelopment())
            {
                var response = new
                {
                    context.Response.StatusCode,
                    Title = "Hello from Exception Handler [Development]",
                    ex.StackTrace
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
            else
            {
                var response = new
                {
                    context.Response.StatusCode,
                    Title = "Hello from Exception Handler [Production]"
                };
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}

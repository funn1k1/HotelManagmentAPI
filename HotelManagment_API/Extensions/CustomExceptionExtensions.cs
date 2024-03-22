using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Newtonsoft.Json;

namespace HotelManagment_API.Extensions
{
    public static class CustomExceptionExtensions
    {
        public static void HandleException(this IApplicationBuilder app, bool isDevelopment)
        {
            app.UseExceptionHandler((app) => app.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var feature = context.Features.GetRequiredFeature<IExceptionHandlerFeature>();
                if (isDevelopment)
                {
                    var response = new
                    {
                        context.Response.StatusCode,
                        Title = "Hello from Exception Handler [Development]",
                        feature.Error.StackTrace
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
            }));
        }
    }
}

using Asp.Versioning;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagment_API.Controllers
{
    [Route("ErrorHandling")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiVersionNeutral]
    [ApiController]
    public class ErrorHandlingAPIController : ControllerBase
    {
        [HttpGet("handleError")]
        public IActionResult HandleError([FromServices] IHostEnvironment enrivonment)
        {
            if (enrivonment.IsDevelopment())
            {
                var feature = HttpContext.Features.GetRequiredFeature<IExceptionHandlerFeature>();
                return Problem(
                    detail: feature.Error.StackTrace,
                    instance: enrivonment.EnvironmentName,
                    title: feature.Error.Message
                );
            }
            return Problem(instance: enrivonment.EnvironmentName);
        }
    }
}

using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HotelManagment_API
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\""
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] {}
                }
            });

            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersion(description));
            }
        }

        private OpenApiInfo CreateVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = "HotelManagment_API",
                Version = description.ApiVersion.ToString(),
                Description = "This is Hotel Managment system",
                Contact = new OpenApiContact
                {
                    Url = new Uri("https://github.com/funn1k1/HotelManagmentAPI"),
                    Email = "2222asd11@list.ru",
                    Name = "Eugene"
                },
                TermsOfService = new Uri("https://github.com/funn1k1/HotelManagmentAPI")
            };

            if (description.IsDeprecated)
            {
                info.Description = "This API version has been deprecated. Please use one of the new APIs available from the explorer";
            }

            return info;
        }
    }
}

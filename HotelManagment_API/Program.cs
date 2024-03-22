using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using HotelManagment_API;
using HotelManagment_API.Data;
using HotelManagment_API.Middlewares;
using HotelManagment_API.Models;
using HotelManagment_API.Repository;
using HotelManagment_API.Repository.Interfaces;
using HotelManagment_API.Services;
using HotelManagment_API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateIssuer = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(configuration["Jwt:SecurityKey"])),
            ValidateIssuerSigningKey = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
);

// Repositories
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

// Services
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddControllers(options =>
{
    // Add caching
    var profiles = configuration.GetSection("CacheProfiles").GetChildren();
    foreach (var profile in profiles)
    {
        options.CacheProfiles.Add(profile.Key, profile.Get<CacheProfile>());
    }

    //options.ReturnHttpNotAcceptable = true;

    // Add filters
    //options.Filters.Add<CustomExceptionFilter>();
}).AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
).ConfigureApiBehaviorOptions(options =>
{
    //options.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData
    //{
    //    Link = "https://github.com/funn1k1/HotelManagmentAPI"
    //};
});/*.AddXmlDataContractSerializerFormatters();*/

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("x-api-version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
    options.AddApiVersionParametersWhenVersionNeutral = true;
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();

builder.Services.AddResponseCaching();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // Add Swagger endpoints
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions.Reverse())
        {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpper()
            );
        }
    });
}

// Initialize the database
await InitDb(app);
//app.UseExceptionHandler("/ErrorHandling/handleError");
//app.HandleException(builder.Environment.IsDevelopment());
app.UseMiddleware<CustomExceptionMiddleware>();
//app.UseSerilogRequestLogging();
app.UseStaticFiles();
app.UseCors();
app.UseHttpsRedirection();
app.UseResponseCaching();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

static async Task InitDb(IApplicationBuilder app)
{
    try
    {
        await DbInitializer.InitAsync(app);
    }
    catch
    {
        // Some logic to handle errors
    }
}

//void HandleException(IApplicationBuilder app)
//{
//    app.Run(async context =>
//    {
//        context.Response.ContentType = "application/json";
//        var feature = context.Features.GetRequiredFeature<IExceptionHandlerFeature>();
//        if (builder.Environment.IsDevelopment())
//        {
//            var response = new
//            {
//                context.Response.StatusCode,
//                Title = "Hello from Exception Handler [Development]",
//                feature.Error.StackTrace
//            };
//            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
//        }
//        else
//        {
//            var response = new
//            {
//                context.Response.StatusCode,
//                Title = "Hello from Exception Handler [Production]"
//            };
//            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
//        }
//    });
//}
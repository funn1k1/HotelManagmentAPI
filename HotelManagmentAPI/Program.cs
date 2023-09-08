using HotelManagmentAPI.Logging;
using HotelManagmentAPI.Logging.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// You can add Serilog capabilities
//Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
//    .WriteTo.File("logs/hotelLog-.txt", rollingInterval: RollingInterval.Day)
//    .WriteTo.Console()
//    .CreateLogger();

////Log.Logger = new LoggerConfiguration()
////    .ReadFrom.Configuration(builder.Configuration)
////    .CreateLogger();

//builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddSingleton<ILogging, Logging>();
builder.Services.AddControllers(options =>
{
    //options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson();/*.AddXmlDataContractSerializerFormatters();*/
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

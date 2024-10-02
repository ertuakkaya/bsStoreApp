using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using Presentation.ActionFilters;
using Repositories.EFCore;
using Services.Contracts;
using WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);


LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true; // ��erik pazarl���na uygunluk 
    config.ReturnHttpNotAcceptable = true; // Kabul edimeyenlere 406 Not Acceptlable ile d�n
})
    .AddCustomCSVFormatter()
    .AddNewtonsoftJson()
    .AddApplicationPart(typeof(Presentation.AssemblyReferance).Assembly)
    .AddXmlDataContractSerializerFormatters(); // XML format�nda veri d�nmek i�in





builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});




builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureLoggerServive();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();


/**
 * 
 *  app build ediltikten sonra logger'� almak i�in servis sa�lay�c�s�n� kullan�yoruz.
 * 
*/
var app = builder.Build();

// logger'� lmak i�in servis sa�lay�c�s�n� kullan�yoruz.
var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExceptionHandler(logger);


//app.UseSwagger();
//app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


if (app.Environment.IsProduction())
{
    app.UseHsts();    
}

app.UseHttpsRedirection();


app.UseCors("CorsPolicy");


app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();


using Microsoft.EntityFrameworkCore;
using ReactChat.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Configuration.AddUserSecrets<Program>();
var configuration = builder.Configuration;

// Configuration

//AppSettings
//var connectionString = configuration.GetConnectionString("DefaultConnection");
//var hangFireConnectionString = configuration.GetConnectionString("HangFireConnection");
//var seqServer = configuration.GetConnectionString("SeqConnection");

//UserSecrets
var connectionString = configuration["ConnectionStrings:DefaultConnection"];
var hangFireConnectionString = configuration["ConnectionStrings:HangFireConnection"];
var seqServer = configuration["ConnectionStrings:SeqConnection"];
if (connectionString == null || hangFireConnectionString == null || seqServer == null)
{
    builder.Configuration.Sources.Clear();
    builder.Configuration
    .AddJsonFile(Path.Combine(builder.Environment.ContentRootPath, "appsettings.json"), optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
    connectionString = configuration.GetConnectionString("DefaultConnection");
    hangFireConnectionString = configuration.GetConnectionString("HangFireConnection");
    seqServer = configuration.GetConnectionString("SeqConnection");
}
// Configure Logging
builder.ConfigureLogging(connectionString!, seqServer!);

// Configure Services
builder.Services.ConfigureHangfire(hangFireConnectionString!);
builder.Services.ConfigureAuthentication(configuration);
builder.Services.ConfigureServices(configuration);

var app = builder.Build();

// Middleware
app.ConfigureMiddleware();

// Configure Endpoints
app.ConfigureEndpoints();

app.Run();
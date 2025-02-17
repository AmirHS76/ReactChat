using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ReactChat.Application.Services.BackgroundService;
using ReactChat.Presentation.Controllers.Hub;
using ReactChat.Presentation.Extensions;
using Serilog;

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

// Configure Logging
builder.ConfigureLogging(connectionString!, seqServer!);

// Configure Services
builder.Services.ConfigureHangfire(hangFireConnectionString!);
builder.Services.ConfigureAuthentication(configuration);
builder.Services.ConfigureServices(configuration);

var app = builder.Build();

// Middleware
app.UseDefaultFiles();
app.UseCors("AllowAll");
app.UseHangfireDashboard();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("/index.html");
app.MapHub<ChatHub>("/chatHub");
app.MapGet("/", () => "----REACT CHAT----");
var a = configuration["Authentication:Google:ClientId"]!;
var b = configuration["Authentication:Google:ClientSecret"]!;
//app.Use(async (context, next) =>
//{
//    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
//    context.Response.Headers.TryAdd("Content-Security-Policy", "frame-ancestors 'none'");
//    await next();
//});
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecksUI(app => app.UIPath = "/health-ui");

// Background Jobs
var messageProcessingService = app.Services.GetRequiredService<MessageProcessingService>();
RecurringJob.AddOrUpdate(
    "process-messages",
    () => messageProcessingService.ProcessMessages(),
    Cron.Minutely);

app.Run();
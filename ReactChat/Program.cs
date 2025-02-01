using Hangfire;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ReactChat.Application.Services.BackgroundService;
using ReactChat.Presentation.Controllers.Hub;
using ReactChat.Presentation.Extensions;
using Serilog;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddHttpContextAccessor();

// Configuration
var connectionString = configuration.GetConnectionString("DefaultConnection");
var hangFireConnectionString = configuration.GetConnectionString("HangFireConnection");
var seqServer = configuration.GetConnectionString("SeqConnection");

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
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    context.Response.Headers.TryAdd("Content-Security-Policy", "frame-ancestors 'none'");
    await next();
});
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description,
                duration = e.Value.Duration.ToString()
            })
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true }));
    }
});

// Background Jobs
var messageProcessingService = app.Services.GetRequiredService<MessageProcessingService>();
RecurringJob.AddOrUpdate(
    "process-messages",
    () => messageProcessingService.ProcessMessages(),
    Cron.Minutely);

app.Run();
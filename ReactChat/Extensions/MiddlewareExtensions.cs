using Hangfire;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using ReactChat.Application.Services.BackgroundService;
using ReactChat.Presentation.Controllers.Hub;
using Serilog;

namespace ReactChat.Presentation.Extensions
{
    public static class MiddlewareExtensions
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
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
        }

        public static void ConfigureEndpoints(this WebApplication app)
        {
            app.MapControllers();
            // Configure API versioning

            app.MapFallbackToFile("/index.html");
            app.MapHub<ChatHub>("/chatHub");
            app.MapGet("/", () => "----REACT CHAT----");
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
        }
    }
}

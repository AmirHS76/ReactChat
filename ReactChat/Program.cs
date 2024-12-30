using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReactChat.Application.Interfaces.Cache;
using ReactChat.Application.Interfaces.MessageHistory;
using ReactChat.Application.Interfaces.MessageHub;
using ReactChat.Application.Interfaces.Register;
using ReactChat.Application.Interfaces.User;
using ReactChat.Application.Mapping;
using ReactChat.Application.Services.BackgroundService;
using ReactChat.Application.Services.Cache;
using ReactChat.Application.Services.Login;
using ReactChat.Application.Services.MessageHistory;
using ReactChat.Application.Services.MessageHub;
using ReactChat.Application.Services.Register;
using ReactChat.Application.Services.User;
using ReactChat.Helpers.HubHelpers;
using ReactChat.Infrastructure.Data.Context;
using ReactChat.Infrastructure.Data.UnitOfWork;
using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.User;
using ReactChat.Infrastructure.Repositories.Users;
using ReactChat.Presentation.Controllers.Hub;
using ReactChat.Presentation.Helpers.HubHelpers;
using Serilog;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var hangFireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.MSSqlServer(
    connectionString: connectionString,
    sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
    {
        TableName = "Logs",
        AutoCreateSqlTable = true
    },
    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddHangfire(config =>
config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
.UseSimpleAssemblyNameTypeSerializer()
.UseRecommendedSerializerSettings()
.UseSqlServerStorage(hangFireConnectionString));

builder.Services.AddHangfireServer();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/chatHub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
builder.Host.UseSerilog();
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("ReactChat.Infrastructure"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageHubService, MessageHubService>();
builder.Services.AddScoped<IMessageHubHelper, MessageHubHelper>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
            //builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials()
                   .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            builder.WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
        });
});
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});
builder.Services.AddSingleton<MessageProcessingService>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
    options.InstanceName = "ReactChat_";
});
//builder.Services.AddHostedService(provider => provider.GetRequiredService<MessageProcessingService>());
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
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

var messageProcessingService = app.Services.GetRequiredService<MessageProcessingService>();
RecurringJob.AddOrUpdate(
    "process-messages",
    () => messageProcessingService.ProcessMessages(),
    Cron.Minutely);

app.Run();
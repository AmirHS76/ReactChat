using Asp.Versioning;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ReactChat.Application.Features.User.Queries.GetAll;
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
using ReactChat.Infrastructure.Data.Context;
using ReactChat.Infrastructure.Data.UnitOfWork;
using ReactChat.Infrastructure.Logging.Enrichers;
using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.User;
using ReactChat.Presentation.Helpers.HubHelpers;
using Serilog;
using StackExchange.Redis;
using System.Reflection;
using System.Text;


namespace ReactChat.Presentation.Extensions;
public static class ServiceExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder, string connectionString, string seqServer)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.With(new UserNameEnricher(builder.Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>()))
            .WriteTo.Console()
            .WriteTo.MSSqlServer(connectionString, new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = true
            }, restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning)
            .WriteTo.Seq(seqServer)
            .CreateLogger();
        builder.Host.UseSerilog();
    }

    public static void ConfigureHangfire(this IServiceCollection services, string hangFireConnectionString)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(hangFireConnectionString));
        services.AddHangfireServer();
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = "External";
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chatHub"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            })
             .AddCookie("External")
             .AddGoogle(options =>
             {
                 options.ClientId = configuration["Authentication:Google:ClientId"]!;
                 options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
             });
        ;
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {


        services.AddHttpContextAccessor();
        services.AddAuthorization();
        services.AddControllers();
        services.AddSignalR();
        services.AddDbContext<UserContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("ReactChat.Infrastructure")));
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
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
        services.AddScoped<LoginService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMessageHubService, MessageHubService>();
        services.AddScoped<IMessageHubHelper, MessageHubHelper>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.WithOrigins("http://localhost:8080", "http://localhost:5173")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
            });
        });
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            Assembly.GetExecutingAssembly(), typeof(GetAllUsersQuery).Assembly));
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });
        services.AddSingleton<MessageProcessingService>();
        services.AddControllers();
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddMvc();

        try
        {
            var redisConnection = ConnectionMultiplexer.Connect(configuration["RedisConnection"]!);
            services.AddSingleton<IConnectionMultiplexer>(redisConnection);
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
                options.InstanceName = "ReactChat_";
            });
            services.AddScoped<ICacheService, DistributedCacheService>();
        }
        catch (Exception ex)
        {
            Log.Logger.Warning("Could not connect to Redis. Falling back to in-memory caching. Exception: {Exception}", ex);
            services.AddMemoryCache();
            services.AddScoped<ICacheService, MemoryCacheService>();
        }
        services.AddHealthChecks()
             .AddCheck("self", () => HealthCheckResult.Healthy())
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!, name: "SQLSERVER")
            .AddRedis(configuration.GetConnectionString("RedisConnection")!, name: "Redis Cache")
            .AddHangfire(hangfireOptions => { hangfireOptions.MinimumAvailableServers = 1; })
            .AddDiskStorageHealthCheck(setup => { setup.AddDrive("C:\\", 1024); });

        services.AddHealthChecksUI()
            .AddInMemoryStorage();
    }
}
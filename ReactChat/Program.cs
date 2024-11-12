using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReactChat.Application.Interfaces.MessageHub;
using ReactChat.Application.Interfaces.Register;
using ReactChat.Application.Interfaces.Users;
using ReactChat.Application.Services.BackgroundServices;
using ReactChat.Application.Services.Login;
using ReactChat.Application.Services.MessageHub;
using ReactChat.Application.Services.Register;
using ReactChat.Application.Services.Users;
using ReactChat.Controllers.Hub;
using ReactChat.Helpers.HubHelpers;
using ReactChat.Infrastructure.Data.Context;
using ReactChat.Infrastructure.Data.UnitOfWork;
using ReactChat.Infrastructure.Repositories;
using ReactChat.Infrastructure.Repositories.Users;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
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
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddDbContext<UserContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("ReactChat.Infrastructure"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRegisterService, RegisterService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageHubService, MessageHubService>();
builder.Services.AddScoped<IMessageHubHelper, MessageHubHelper>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
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
builder.Services.AddHostedService(provider => provider.GetRequiredService<MessageProcessingService>());
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");
app.MapHub<ChatHub>("/chatHub");
app.MapGet("/", () => "----REACT CHAT----");

app.Run();

﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ReactChat.Application.Features.User.Queries.GetByUsername;
using ReactChat.Application.Features.UserSessions.Commands;
using ReactChat.Application.Features.UserSessions.Queries;
using ReactChat.Core.Entities.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactChat.Application.Services.Login
{
    public class LoginService(IMediator mediator)
    {
        private readonly string refreshTokenSecretKey = "UHyuZrvwyjfv9j0dgOGINMXRqiHzSeTlF+uYPsep2Dg=";
        private readonly IMediator _mediator = mediator;

        public async Task<string?> Authenticate(string username, string password, CancellationToken cancellationToken, HttpContext httpContext)
        {
            BaseUser? user = await _mediator.Send(new GetUserByUsernameQuery(username), cancellationToken);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                if (!await AddUserSession(user.Id, httpContext, cancellationToken))
                    return null;
                var token = GenerateJwtTokenFromUser(user);
                return token;
            }
            return null;
        }

        public async Task<string?> ValidateRefreshToken(string token, CancellationToken cancellationToken)
        {
            var refreshToken = token;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(refreshTokenSecretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out var validatedToken);

                string? username = principal.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
                var user = await _mediator.Send(new GetUserByUsernameQuery(username ?? ""), cancellationToken);
                if (user is null)
                    return GenerateTokensForGoogleUser(principal).token;
                else
                    return GenerateJwtTokenFromUser(user);
                return null;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GenerateJwtTokenFromUser(BaseUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username ?? throw new NullReferenceException()),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dDgM0xSLiiYjqF+U4TkygUYjaDWdE68RLkilOHTzHrY="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(refreshTokenSecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity([new Claim("username", username)]),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public (string token, string refreshToken) GenerateTokensForGoogleUser(ClaimsPrincipal principal)
        {
            var username = principal.FindFirst(ClaimTypes.Name)?.Value ?? principal.FindFirst("username")?.Value ?? throw new NullReferenceException("Username not found");
            var email = principal.FindFirst(ClaimTypes.Email)?.Value ?? "";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role,"RegularUser")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dDgM0xSLiiYjqF+U4TkygUYjaDWdE68RLkilOHTzHrY="));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "YourIssuer",
                audience: "YourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = GenerateRefreshToken(username);

            return (jwtToken, refreshToken);
        }
        private async Task<bool> AddUserSession(int userId, HttpContext httpContext, CancellationToken cancellationToken = default)
        {
            var userIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var currentSession = await _mediator.Send(new GetUserSessionsQuery(userId, userIp), cancellationToken);
            if (currentSession != null && currentSession.Count > 0)
            {
                var userSession = currentSession.FirstOrDefault() ?? throw new NullReferenceException("User session was invalid");
                userSession.LastActivity = DateTime.Now;
                userSession.IsRevoked = false;
                return await _mediator.Send(new UpdateUserSessionCommand(userSession), cancellationToken);
            }
            var session = new UserSession
            {
                UserId = userId.ToString(),
                UserAgent = httpContext.Request.Headers["User-Agent"].ToString(),
                IpAddress = userIp
            };
            return await _mediator.Send(new CreateUserSessionCommand(session), cancellationToken);
        }
    }
}

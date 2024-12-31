using Microsoft.IdentityModel.Tokens;
using ReactChat.Core.Entities.User;
using ReactChat.Infrastructure.Data.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ReactChat.Application.Services.Login
{
    public class LoginService(IUnitOfWork userUnitOfWork)
    {
        private readonly string refreshTokenSecretKey = "UHyuZrvwyjfv9j0dgOGINMXRqiHzSeTlF+uYPsep2Dg=";
        private readonly IUnitOfWork _unitOfWork = userUnitOfWork;

        public async Task<string?> Authenticate(string username, string password)
        {
            var userRepository = _unitOfWork.UserRepository;
            BaseUser? user = await userRepository.GetUserByUsernameAsync(username);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                var token = GenerateJwtToken(user);
                return token;
            }
            return null;
        }

        public async Task<string?> ValidateRefreshToken(string token)
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
                var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username ?? throw new MissingFieldException("User not found"));
                return user == null ? null : GenerateJwtToken(user);

            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GenerateJwtToken(BaseUser user)
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
    }
}

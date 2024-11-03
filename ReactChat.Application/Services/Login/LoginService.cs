using Microsoft.IdentityModel.Tokens;
using ReactChat.Core.Entities.Login;
using ReactChat.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ReactChat.Application.Services.Login
{
    public class LoginService
    {
        IUnitOfWork _unitOfWork;
        public LoginService(IUnitOfWork userUnitOfWork)
        {
            _unitOfWork = userUnitOfWork;
        }
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
        private string GenerateJwtToken(BaseUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
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
    }
}

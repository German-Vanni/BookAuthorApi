using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookAuthor.Api.Services.AuthManager
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateToken(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettingsSection = _configuration.GetSection("Jwt");
            var key = Environment.GetEnvironmentVariable("ASPNET_API_SECRET");
            if (key == null)
            {
                key = jwtSettingsSection.GetSection("Key").Value;
            }
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        public async Task<bool> ValidateUser(UserDtoForLogin userDto)
        {
            _user = await _userManager.FindByNameAsync(userDto.Email);
            if(_user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password))
            {
                return true;
            }
            return false;
        }

        private JwtSecurityToken GenerateToken(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettingsSection = _configuration.GetSection("Jwt");
            var expireDate = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettingsSection.GetSection("Lifetime").Value));

            var token = new JwtSecurityToken(
                    issuer: jwtSettingsSection.GetSection("Issuer").Value,
                    claims: claims,
                    expires: expireDate,
                    signingCredentials: signingCredentials
                );

            return token;
        }
    }
}

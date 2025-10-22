using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehiclePortal.Models;

namespace VehiclePortal.Service
{
    public class JwtHelper
    {
        private readonly string _key;
        public JwtHelper(string key) => _key = key;

        public string GenerateToken(Userlogin user, int expireMinutes = 60)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? ""),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.LoginRole ?? ""),
                new Claim("districtId", user.DistrictId?.ToString() ?? ""),
                 new Claim("DistrictName", user.District?.DistrictName ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

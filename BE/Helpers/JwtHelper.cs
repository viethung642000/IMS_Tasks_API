using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using BE.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BE.Helpers
{
    public class JwtHelper
    {
        private readonly JwtSetting jwtSetting;

        public JwtHelper(IOptionsMonitor<JwtSetting> jwtSetting)
        {
            this.jwtSetting = jwtSetting.CurrentValue;
        }

        public async Task<string> GenerateToken()
        {
            var handler = new JwtSecurityTokenHandler();

            var description = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim(ClaimTypes.Email, "abc@abc.com"),
                    new Claim(ClaimTypes.Name, "abc"),
                    new Claim(ClaimTypes.NameIdentifier, "1"),
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Secret)), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddMinutes(1)
            };

            var token = handler.CreateToken(description);
            return await Task.FromResult(handler.WriteToken(token));
        }
    }
}
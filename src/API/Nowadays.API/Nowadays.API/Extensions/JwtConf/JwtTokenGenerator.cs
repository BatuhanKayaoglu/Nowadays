using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Nowadays.API.Extensions.JwtConf
{
    public static class JwtTokenGenerator
    {
        public static TokenResponseDto GenerateToken(LoginUserViewModel dto) 
        {
            Claim[] claims = new Claim[]
           {
                     // All of this information will be found in Jwt Token.
                     new Claim(ClaimTypes.NameIdentifier,dto.Id.ToString()),
                     new Claim(ClaimTypes.GivenName,dto.NameSurname),
           };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            DateTime expireDate = DateTime.UtcNow.AddDays(JwtTokenDefaults.Expire);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(issuer: JwtTokenDefaults.ValidIssuer, audience: JwtTokenDefaults.ValidAudience, claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddMinutes(JwtTokenDefaults.Expire), signingCredentials: credentials);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            // RefreshToken
            byte[] numbers = new byte[32];
            using RandomNumberGenerator random = RandomNumberGenerator.Create();
            random.GetBytes(numbers);
            var refreshToken = Convert.ToBase64String(numbers);

            var accessToken = handler.WriteToken(jwtSecurityToken);

            return new TokenResponseDto(accessToken, expireDate, refreshToken);
        }
    }
}

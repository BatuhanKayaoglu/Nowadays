using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Common.ViewModels
{
    public class TokenResponseDto
    {
        public TokenResponseDto(string token, DateTime expireDate, string refreshToken)
        {
            Token = token;
            ExpireDate = expireDate;
            RefreshToken = refreshToken;
        }

        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }

        public string RefreshToken { get; set; }
    }
}

using Nowadays.Common.ViewModels;

namespace Nowadays.API.Services.Token
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GenerateJwtToken(LoginUserViewModel dto);
    }
}
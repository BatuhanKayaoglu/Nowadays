using AutoMapper;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Nowadays.API.Services.Auth;
using Nowadays.API.Services.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;


namespace Nowadays.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController(IAuthService authService, IUnitOfWork uow, ITokenService tokenService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _uow = uow;

        [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(CreateUserResponseViewModel user)
        {
            IdentityResult result = await _authService.Register(user);

            // For email confirmation   
            await _authService.GenerateConfirmEmailTokenAndSendMail(user.Email);

            if (result.Succeeded)
                return Ok("User registered successfully and Confirmation email sent.");
            else
                return BadRequest("Failed to register user.");
        }


        [HttpPost]
        [Route("Login")]
        public async Task<LoginUserViewModel> Login(LoginUserResponseViewModel user)
        {
            bool check2FA = await _authService.IsTwoFactorEnabled(user.EmailAddress);
            if (check2FA)
            {
                await _authService.GenerateTwoFactorToken(user.EmailAddress);
                return new LoginUserViewModel { NameSurname = user.EmailAddress };
            }

            LoginUserViewModel result = await _authService.Login(user);

            TokenResponseDto responseJwt = await _tokenService.GenerateJwtToken(result);
            result.Token = responseJwt.Token;
            result.RefreshToken = responseJwt.RefreshToken;

            return result;
        }


        // For testing  
        [HttpPost]
        [Route("GenerateTestJwtToken")]
        public async Task<LoginUserViewModel> GenerateToken()
        {
            LoginUserViewModel lgn= new LoginUserViewModel();
            lgn.NameSurname = "batu";
            lgn.Id=Guid.NewGuid().ToString();  
            TokenResponseDto responseJwt = await _tokenService.GenerateJwtToken(lgn);
            lgn.Token = responseJwt.Token;
            lgn.RefreshToken = responseJwt.RefreshToken;

            return lgn;
        }





        [HttpGet]
        [Route("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail([Required] string email, string token)
        {
            IdentityResult result = await _authService.ConfirmEmail(email, token);
            if (result.Succeeded)
                return Ok("Email confirmed successfully.");
            else
                return BadRequest($"Email not confirmed => {result.Errors.First()}");
        }


        [HttpPost]
        [Route("Forgot-Password")]
        public async Task ForgotPassword([Required] string email)
        {
            await _authService.ForgotPassword(email);
        }

        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([Required] string email, string token, string newPassword)
        {
            IdentityResult result = await _authService.ResetPassword(email, token, newPassword);
            if (result.Succeeded)
                return Ok("Password reset successfully.");
            else
                return BadRequest($"Password not reset => {result.Errors.First()}");
        }

        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> LogOut(string email)
        {
            IdentityResult result = await _authService.LogOut(email);
            if (result.Succeeded)
                return Ok("User signed out successfully.");
            else
                return BadRequest("Failed to sign out user.");
        }

        [HttpPost]
        [Route("Change-Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassModel)
        {
            var result = await _authService.ChangePassword(changePassModel);
            if (result.Succeeded)
                return Ok("Password changed successfully.");
            else
                return BadRequest("Failed to change password.");
        }

        [HttpPost]
        [Route("Login-2FA")]
        public async Task<IActionResult> LoginWith2FA(string code)
        {
            var result = await _authService.LoginWith2FA(code);
            if (result.Succeeded)
                return Ok("Login with 2FA successfully.");
            else
                return BadRequest("Failed to Login with 2FA.");
        }

    }
}

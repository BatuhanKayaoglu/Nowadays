using AutoMapper;
using Azure.Core;
using Nowadays.Common.Extensions;
using Nowadays.Common.ResponseViewModel;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using Nowadays.Entity.Models.Identity;
using Nowadays.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nowadays.API.Exceptions;
using Nowadays.API.Extensions.JwtConf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Nowadays.API.Services.EmailSender;

namespace Nowadays.API.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, IUnitOfWork uow, IMapper mapper, IEmailSenderService emailSender, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _uow = uow;
            _mapper = mapper;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> Register(CreateUserResponseViewModel user)
        {
            bool isExist = await _userManager.Users.AnyAsync(x => x.Email == user.Email);
            if (isExist)
                throw new DatabaseValidationException("User already exists");

            IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                NameSurname = user.NameSurname,
                UserName = user.NameSurname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber

            }, user.Password);

            if (result.Succeeded)
            {
                return result;
            }
            string errorList = string.Join(" - ", result.Errors.Select(error => error.Description));
            throw new UserCreateFailedException("User not created!", new Exception(errorList));
        }

        public async Task<LoginUserViewModel> Login(LoginUserResponseViewModel user)
        {
            AppUser? dbUser = await _userManager.FindByEmailAsync(user.EmailAddress);
            if (dbUser == null)
                throw new DatabaseValidationException("User not found");

            bool checkPass = await _userManager.CheckPasswordAsync(dbUser!, user.Password);
            if (!checkPass)
            {
                int accessFailedCount = await _userManager.GetAccessFailedCountAsync(dbUser);
                if (accessFailedCount > 3)
                    await _userManager.SetLockoutEndDateAsync(dbUser!, DateTimeOffset.Now.AddMinutes(5));  // Can be tried again after 5 minutes.  

                throw new DatabaseValidationException("Password is wrong");
            }

            LoginUserViewModel result = new LoginUserViewModel
            {
                Id = dbUser!.Id,
                NameSurname = dbUser.NameSurname,
            };

            if (dbUser.TwoFactorEnabled)
            {
                string token = await _userManager.GenerateTwoFactorTokenAsync(dbUser!, "Email");
                await _emailSender.SendEmailAsync(dbUser.Email!, "Two Factor Authentication", token);
            }

            var signInAccount = await _signInManager.PasswordSignInAsync(dbUser, user.Password, false, lockoutOnFailure: false); // logging in and marking the user as logged in.  
            if (signInAccount.Succeeded)
                return result;
            else
                throw new DatabaseValidationException("User not allowed");
        }

        public async Task GenerateConfirmEmailTokenAndSendMail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user!);

            HttpRequest request = _httpContextAccessor.HttpContext!.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            var confirmationLink = $"{baseUrl}/api/auth/ConfirmEmail?email={email}&token={token}";  //"https://localhost:44308/api/auth/ConfirmEmail?email={email}&token={token}"
            var message = confirmationLink;

            await _emailSender.SendEmailAsync(email, "Confirm your email", message);
        }

        public async Task<IdentityResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new DatabaseValidationException("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result;
        }


        public async Task ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new DatabaseValidationException("User not found");

            var userMail = user.Email;

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            HttpRequest request = _httpContextAccessor.HttpContext!.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            string confirmationLink = $"{baseUrl}/api/auth/ResetPassword?email={email}&token={token}";
            string message = confirmationLink;

            await _emailSender.SendEmailAsync(userMail!, "Reset your email", message);
        }

        public async Task<IdentityResult> ResetPassword(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new DatabaseValidationException("User not found");

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result;
        }

        public async Task<IdentityResult> LogOut(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new DatabaseValidationException("User not found");

            await _signInManager.SignOutAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> ChangePassword(ChangePasswordViewModel changePassModel)
        {
            if (changePassModel.email == null)
                throw new DatabaseValidationException("Email is required");

            var user = await _userManager.FindByEmailAsync(changePassModel.email!);
            if (user == null)
                throw new DatabaseValidationException("User not found");

            IdentityResult result = await _userManager.ChangePasswordAsync(user!, changePassModel.oldPassword!, changePassModel.newPassword!);
            return result;
        }

        public async Task<bool> IsTwoFactorEnabled(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new DatabaseValidationException("User not found");
            return await _userManager.GetTwoFactorEnabledAsync(user);
        }

        public async Task GenerateTwoFactorToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            string token = await _userManager.GenerateTwoFactorTokenAsync(user!, "Email");

            await _emailSender.SendEmailAsync(email!, "Two Factor Authentication", token);
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginWith2FA(string code)
        {
            var result = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);
            return result;
        }



    }
}

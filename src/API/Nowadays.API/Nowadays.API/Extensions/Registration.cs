﻿using Nowadays.Infrastructure.Context;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.Infrastructure.Repositories;
using Nowadays.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Nowadays.API.Extensions.JwtConf;
using System.Reflection;
using System.Text;
using Nowadays.API.Services.Auth;
using Nowadays.API.Services.Token;
using Nowadays.API.Services.EmailSender;
using Nowadays.API.Middlewares.ExceptionHandlerMiddleware;
using Nowadays.API.Middlewares.Filter.Validation;

namespace Nowadays.API.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddAPIRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ICompanyService, CompanyService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IIssueService, IssueService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IReportService, ReportService>();

            // **** JWT CONFIGURATION START ****
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = JwtTokenDefaults.ValidAudience,
                    ValidIssuer = JwtTokenDefaults.ValidIssuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key)),
                    ValidateIssuerSigningKey = true, // To verify whether the token value belongs to our application.
                    ValidateLifetime = true
                };

            });
            // **** JWT CONFIGURATION END ****

            // ****VALIDATION CONFIGURATION START ****  
            services.AddTransient<ExceptionMiddleware>();

            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
            });
            // ****VALIDATION CONFIGURATION END ****  


            return services;

        }
    }
}

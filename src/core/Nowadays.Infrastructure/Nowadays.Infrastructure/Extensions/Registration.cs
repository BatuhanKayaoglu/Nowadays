using Nowadays.Entity.Models.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nowadays.Infrastructure.Context;
using Nowadays.Infrastructure.IRepositories;
using Nowadays.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NowadaysContext>(conf =>
            {
                var connStr = configuration["NowadaysDbConnectionString"].ToString();
                conf.UseSqlServer(connStr);
            });

            var assm = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assm);
            services.AddValidatorsFromAssembly(assm);

            services.AddIdentity<AppUser, AppRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<NowadaysContext>(); // Identity'yi ekledik.   

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;

        }
    }
}

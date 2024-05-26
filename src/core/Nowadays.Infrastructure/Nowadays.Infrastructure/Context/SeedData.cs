using Bogus;
using Nowadays.Common.Extensions;
using Nowadays.Common.ViewModels;
using Nowadays.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Context
{
    public class SeedData
    {
        public List<Company> Companies { get; private set; }
        public List<Employee> Employees { get; private set; }
        public List<Project> Projects { get; private set; }
        public List<Issue> Issues { get; private set; }

        public SeedData()
        {
            Companies = GenerateCompanies(5);
            Employees = GenerateEmployees(20);
            Projects = GenerateProjects(Companies, 10);
            Issues = GenerateIssues(Projects, Employees, 30);
        }

        public async Task SeedAsync(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<NowadaysContext>();
            optionsBuilder.UseSqlServer(configuration["NowadaysDbConnectionString"]);



            using (var context = new NowadaysContext(optionsBuilder.Options))
            {
                // Clear the database   
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();

                // Add seed data    
                await context.Companies.AddRangeAsync(Companies);
                await context.Employees.AddRangeAsync(Employees);
                await context.Projects.AddRangeAsync(Projects);
                await context.Issues.AddRangeAsync(Issues);

                await context.SaveChangesAsync();
            }
        }

        static List<Company> GenerateCompanies(int count)
        {
            var companyFaker = new Faker<Company>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.CreateDate, f => f.Date.Past(1))
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Projects, f => new List<Project>());

            return companyFaker.Generate(count);
        }

        static List<Employee> GenerateEmployees(int count)
        {
            var employeeFaker = new Faker<Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.CreateDate, f => f.Date.Past(1))
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.Email, f => f.Internet.Email())
                .RuleFor(e => e.Projects, f => new List<Project>())
                .RuleFor(e => e.Issues, f => new List<Issue>());

            return employeeFaker.Generate(count);
        }

        static List<Project> GenerateProjects(List<Company> companies, int count)
        {
            var projectFaker = new Faker<Project>()
                .RuleFor(p => p.Id, f => Guid.NewGuid())
                .RuleFor(p => p.CreateDate, f => f.Date.Past(1))
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.CompanyId, f => f.PickRandom(companies).Id)
                .RuleFor(p => p.Company, (f, p) => companies.Find(c => c.Id == p.CompanyId))
                .RuleFor(p => p.Employees, f => new List<Employee>())
                .RuleFor(p => p.Issues, f => new List<Issue>());

            var projects = projectFaker.Generate(count);

            foreach (var project in projects)
            {
                var company = companies.Find(c => c.Id == project.CompanyId);
                if (company != null)
                    company.Projects.Add(project);
            }

            return projects;
        }

        static List<Issue> GenerateIssues(List<Project> projects, List<Employee> employees, int count)
        {
            var issueFaker = new Faker<Issue>()
                .RuleFor(i => i.Id, f => Guid.NewGuid())
                .RuleFor(i => i.CreateDate, f => f.Date.Past(1))
                .RuleFor(i => i.Title, f => f.Lorem.Sentence())
                .RuleFor(i => i.Description, f => f.Lorem.Paragraph())
                .RuleFor(i => i.ProjectId, f => f.PickRandom(projects).Id)
                .RuleFor(i => i.Project, (f, i) => projects.Find(p => p.Id == i.ProjectId))
                .RuleFor(i => i.Assignees, f => new List<Employee>());

            var issues = issueFaker.Generate(count);

            foreach (var issue in issues)
            {
                var project = projects.Find(p => p.Id == issue.ProjectId);
                if (project != null)
                    project.Issues.Add(issue);

                var assignees = new Faker().PickRandom(employees, 2).ToList();
                foreach (var employee in assignees)
                {
                    issue.Assignees.Add(employee);
                    employee.Issues.Add(issue);
                }
            }

            return issues;
        }
    }
}


using Nowadays.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Context.EntityConfigurations
{
    public class EmployeeConfiguration : BaseEntityConfiguration<Employee>
    {
        public override void Configure(EntityTypeBuilder<Employee> builder)
        {
            base.Configure(builder);

            builder.ToTable("Employee", NowadaysContext.DEFAULT_SCHEMA);

            builder.Property(e => e.Name)
           .IsRequired()
           .HasMaxLength(100);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(e => e.Projects)
                .WithMany(p => p.Employees)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectEmployee",
                    j => j.HasOne<Project>()
                          .WithMany()
                          .HasForeignKey("ProjectId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Employee>()
                          .WithMany()
                          .HasForeignKey("EmployeeId")
                          .OnDelete(DeleteBehavior.Cascade));

            builder.HasMany(e => e.Issues)
                .WithMany(i => i.Assignees)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeIssue",
                    j => j.HasOne<Issue>()
                          .WithMany()
                          .HasForeignKey("IssueId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Employee>()
                          .WithMany()
                          .HasForeignKey("EmployeeId")
                          .OnDelete(DeleteBehavior.Cascade));
        }
    }
}

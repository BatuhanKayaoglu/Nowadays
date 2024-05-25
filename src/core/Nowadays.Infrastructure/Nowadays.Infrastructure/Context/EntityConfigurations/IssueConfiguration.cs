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
    public class IssueConfiguration : BaseEntityConfiguration<Issue>
    {
        public override void Configure(EntityTypeBuilder<Issue> builder)
        {
            base.Configure(builder);

            builder.ToTable("Issue", NowadaysContext.DEFAULT_SCHEMA);

            builder.Property(i => i.Title)
           .IsRequired()
           .HasMaxLength(100);

            builder.Property(i => i.Description)
                .HasMaxLength(1000);

            // Relationships
            builder.HasOne(i => i.Project)
                .WithMany(p => p.Issues)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(i => i.Assignees)
                .WithMany(e => e.Issues)
                .UsingEntity<Dictionary<string, object>>(
                    "EmployeeIssue",
                    j => j.HasOne<Employee>()
                          .WithMany()
                          .HasForeignKey("EmployeeId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Issue>()
                          .WithMany()
                          .HasForeignKey("IssueId")
                          .OnDelete(DeleteBehavior.Cascade));
        }
    }
}


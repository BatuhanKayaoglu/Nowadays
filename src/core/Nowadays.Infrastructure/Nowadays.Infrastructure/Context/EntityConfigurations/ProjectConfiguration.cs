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
    public class ProjectConfiguration : BaseEntityConfiguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);

            builder.ToTable("Project", NowadaysContext.DEFAULT_SCHEMA);

            builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

            builder.HasOne(p => p.Company)
                .WithMany(c => c.Projects)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Employees)
                .WithMany(e => e.Projects)
                .UsingEntity<Dictionary<string, object>>(
                    "ProjectEmployee",
                    j => j.HasOne<Employee>()
                          .WithMany()
                          .HasForeignKey("EmployeeId")
                          .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Project>()
                          .WithMany()
                          .HasForeignKey("ProjectId")
                          .OnDelete(DeleteBehavior.Cascade));

            builder.HasMany(p => p.Issues)
                .WithOne(i => i.Project)
                .HasForeignKey(i => i.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}

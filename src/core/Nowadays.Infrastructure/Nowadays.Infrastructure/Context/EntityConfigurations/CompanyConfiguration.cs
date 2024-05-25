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
    public class CompanyConfiguration : BaseEntityConfiguration<Company>
    {
        public override void Configure(EntityTypeBuilder<Company> builder)
        {
            base.Configure(builder);

            builder.ToTable("Company", NowadaysContext.DEFAULT_SCHEMA);

            builder.Property(c => c.Name)
                        .IsRequired()
                        .HasMaxLength(100);

            builder.Property(c => c.Address)
                .HasMaxLength(250);

            builder.HasMany(c => c.Projects)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId);
        }
    }
}


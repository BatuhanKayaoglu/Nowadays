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
    public class ReportConfiguration : BaseEntityConfiguration<Report>
    {
        public override void Configure(EntityTypeBuilder<Report> builder)
        {
            base.Configure(builder);

            builder.ToTable("Report", NowadaysContext.DEFAULT_SCHEMA);

            builder.Property(r => r.Title)
                       .IsRequired()
                       .HasMaxLength(100);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(r => r.CreatedAt)
                .IsRequired();

        }
    }
}


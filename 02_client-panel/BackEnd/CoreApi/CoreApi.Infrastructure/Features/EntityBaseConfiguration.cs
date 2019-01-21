﻿using CoreApi.Infrastructure.Features.Common;
using CoreApi.Models.Angular;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApi.Infrastructure.Features
{
    public class ClientConfiguration : EntityBaseConfiguration<Client>
    {
        public override void ConfigureDerived(EntityTypeBuilder<Client> builder)
        {
            builder.Property(x => x.Balance).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Phone).HasMaxLength(50);
        }
    }
}

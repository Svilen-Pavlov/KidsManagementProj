using KidsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Configuration
{
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {

            builder
                .HasMany(l => l.Groups)
                .WithOne(g => g.Level)
                .HasForeignKey(g => g.LevelId);
        }
    }
}

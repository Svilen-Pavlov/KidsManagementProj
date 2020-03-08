using KidsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Configuration
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder
                .HasMany(g => g.Students)
                .WithOne(s => s.Group)
                .HasForeignKey(s=>s.GroupId);
                
        }
    }
}

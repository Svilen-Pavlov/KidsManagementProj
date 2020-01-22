using Kids.Management.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Configuration
{
    public class ParentConfiguration : IEntityTypeConfiguration<StudentParent>
    {
        public void Configure(EntityTypeBuilder<StudentParent> builder)
        {
            builder
                .HasKey(sp => new { sp.StudentId, sp.ParentId });
           
            builder
                .HasOne(s => s.Student)
                .WithMany(s => s.Parents);
            
            builder
                 .HasOne(p => p.Parent)
                 .WithMany(p => p.Children);
        }
    }
}

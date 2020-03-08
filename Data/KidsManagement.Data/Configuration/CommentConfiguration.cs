using KidsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Configuration
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder
                .HasOne(c => c.Student)
                .WithMany(s => s.TeacherComments);
            builder
                .HasOne(c => c.Teacher)
                .WithMany(t => t.StudentComments);
        }
    }
}

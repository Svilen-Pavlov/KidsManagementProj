using KidsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Configuration
{
    public class LevelTeacherConfiguration : IEntityTypeConfiguration<LevelTeacher>
    {
        public void Configure(EntityTypeBuilder<LevelTeacher> builder)
        {
            builder.
                HasKey(lt => new {lt.LevelId,lt.TeacherId });
            builder
                .HasOne(lt => lt.Teacher)
                .WithMany(t => t.QualifiedLevels);
            builder
                .HasOne(lt => lt.Level)
                .WithMany(l => l.EligibleTeachers);
        }
    }
}

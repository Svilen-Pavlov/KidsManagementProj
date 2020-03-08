using KidsManagement.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidsManagement.Data.Configuration
{
    public class NoteConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder
                .HasOne(n => n.Admin)
                .WithMany(a => a.AdminNotes)
                .HasForeignKey(n => n.AdminId);
                
            builder
                .HasOne(n=>n.Parent)
                .WithMany(p=>p.AdminNotes)
                .HasForeignKey(n=>n.ParentId);

        }
    }
}

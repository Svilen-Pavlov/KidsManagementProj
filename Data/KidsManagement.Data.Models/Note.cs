using KidsManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kids.Management.Data.Models
{
    public class Note
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Const.textMaxLen)]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int AdminId { get; set; }
        public Admin Admin { get; set; }
        
        [Required]
        public int ParentId { get; set; }
        public Parent Parent { get; set; }
    }
}

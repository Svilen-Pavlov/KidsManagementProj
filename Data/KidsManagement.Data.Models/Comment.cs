using KidsManagement.Data;
using KidsManagement.Data.Models.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KidsManagement.Data.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Constants.Constants.textMaxLen)]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        //middle table between teachers and students 

        public override string ToString()
        {
            return this.Content;
        }
    }
}

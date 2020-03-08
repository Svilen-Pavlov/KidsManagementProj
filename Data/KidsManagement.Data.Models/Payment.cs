using KidsManagement.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KidsManagement.Data.Models
{
    public class Payment
    {
        public Payment()
        {
            this.IsDeleted = false;
        }
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime ReceivalDate { get; set; }
        public DateTime? PaymentDate { get; set; }

        [Required]
        public PaymentType Type { get; set; }

        //in percent
        public double Discount { get; set; }

        public string DiscountBasis { get; set; }

        [Required]
        public int StudentId { get; set; }

        public Student Student { get; set; }


        [Required]
        public DateTime CreatedOn { get; set; }

        public DateTime LastModified { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}

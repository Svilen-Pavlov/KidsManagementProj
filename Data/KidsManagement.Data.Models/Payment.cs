using Kids.Management.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kids.Management.Data.Models
{
    public class Payment
    {
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
        public int Discount { get; set; }

        public string DiscountBasis { get; set; }

        [Required]
        public int StudentId { get; set; }

        public Student Student { get; set; }
    }
}

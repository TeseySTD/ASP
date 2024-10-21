using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Models.Entities
{
    public class Rental
    {
        [Key]
        public int RentalId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime RentalStartDate { get; set; }
        public DateTime RentalEndDate { get; set; }

        public decimal PaymentAmount { get; set; }
    }
}

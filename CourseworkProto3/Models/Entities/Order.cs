using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Models.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }

        public int? ProductId { get; set; }
        public Product? Product { get; set; }

        public DateTime? StartDate { get; set; }  

        public DateTime? EndDate { get; set; } 
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? PaymentAmount { get; set; } 
        public OrderStatus Status { get; set; } 
    }
}

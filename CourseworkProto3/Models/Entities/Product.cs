using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        public string Title { get; set; }

        public ProductType ProductType { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public Disc Disc { get; set; }
        public Book Book { get; set; }
    }

}

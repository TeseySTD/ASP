using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Models.Entities
{
    public class Disc
    {
        [Key]
        public int DiscId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DiscFormat Format { get; set; }
        public int? Year { get; set; }
        public DiscType DiscType { get; set; }

        public Movie Movie { get; set; }
        public Music Music { get; set; }
        public Game Game { get; set; }

    }
}
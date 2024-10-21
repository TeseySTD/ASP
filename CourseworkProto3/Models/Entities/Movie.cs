using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Library.Models.Entities
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [ForeignKey("Disc")]
        public int DiscId { get; set; }
        public Disc Disc { get; set; }

        public int Duration { get; set; }
        public string Director { get; set; }
        public MovieGenre Genre { get; set; }
        public List<Actor> Actors { get; set; } = new();
    }
}
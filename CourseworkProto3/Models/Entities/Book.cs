using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Models.Entities;

public class Book
{
    [Key]
    public int BookId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public string Author { get; set; }
    public BookGenre Genre { get; set; }
    public int PublicationYear { get; set; }
}

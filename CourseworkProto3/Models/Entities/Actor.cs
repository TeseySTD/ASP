using System.ComponentModel.DataAnnotations;

namespace Library.Models.Entities;

public class Actor
{

    [Key]
    public int ActorId { get; set; }
    public string Name { get; set; }

    public List<Movie> Movies { get; set; } = new();
}
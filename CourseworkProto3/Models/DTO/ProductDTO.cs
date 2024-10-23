using System.ComponentModel.DataAnnotations;
using Library.Models.Entities;

namespace Library.Models.DTO;

public class ProductDto
{
    public int ProductId { get; set; }

    [Required(ErrorMessage = "Назва обов'язкова")]
    public string Title { get; set; }

    public BookDto Book { get; set; }

    public DiscDto Disc { get; set; }
}

public class BookDto
{
    [Required(ErrorMessage = "Автор обов'язковий")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Ім'я автора має містити від 3 до 50 символів")]
    public string Author { get; set; }

    [Required(ErrorMessage = "Жанр обов'язковий")]
    [RegularExpression(pattern:"^[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+(,[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+)*$", ErrorMessage = "Неправильний список жанрів")]
    public string Genre { get; set; }

    [Range(1800, 2024, ErrorMessage = "Рік видання має бути між 1800 та 2024")]
    public int PublicationYear { get; set; }
}

public class DiscDto
{
    [Required(ErrorMessage = "Формат обов'язковий")]
    public DiscFormat Format { get; set; }

    [Required(ErrorMessage = "Рік обов'язковий")]
    [Range(1900, 2024, ErrorMessage = "Рік має бути між 1900 та 2024")]
    public int Year { get; set; }
    public MovieDto Movie { get; set; }
    public MusicDto Music { get; set; }
    public GameDto Game { get; set; }
}

public class MovieDto
{
    [Required(ErrorMessage = "Режисер обов'язковий")]
    [StringLength(50, ErrorMessage = "Ім'я режисера не може перевищувати 50 символів")]
    public string Director { get; set; }

    [Range(1, 6000, ErrorMessage = "Тривалість некоректна")]
    public int Duration { get; set; }

    [Required(ErrorMessage = "Жанр обов'язковий")]
    [RegularExpression(pattern:"^[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+(,[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+)*$", ErrorMessage = "Неправильний список жанрів")]
    public string Genre { get; set; }

    [Required(ErrorMessage = "Актори обов'язкові")]
    [RegularExpression(pattern:"^[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+(,[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+)*$", ErrorMessage = "Неправильно введені актори")]
    public string Actors { get; set; } = null!;
}

public class MusicDto
{
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Виконавець має бути від 1 до 255 символів")]
    [Required(ErrorMessage = "Артист обов'язковий")]
    public string Artist { get; set; }

    [Required(ErrorMessage = "Жанр обов'язковий")]
    [RegularExpression(pattern:"^[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+(,[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+)*$", ErrorMessage = "Неправильний список жанрів")]
    public string Genre { get; set; }

    [Required(ErrorMessage = "Кількість треків обов'язкова")]
    [Range(1, 10000, ErrorMessage = "Кількість треків має бути від 1 до 10000")]
    public int TrackCount { get; set; }
}

public class GameDto
{
    [Required(ErrorMessage = "Розробник обов'язковий")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Розробник має бути від 1 до 255 символів")]

    public string Developer { get; set; }

    [Required(ErrorMessage = "Видавець обов'язковий")]
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Видавець має бути від 1 до 255 символів")]
    public string Publisher { get; set; }

    [Required(ErrorMessage = "Жанр обов'язковий")]
    [RegularExpression(pattern:"^[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+(,[a-zA-Zа-яА-ЯЇїІіЄєҐґ\\s]+)*$", ErrorMessage = "Неправильний список жанрів")]
    public string Genre { get; set; }
}

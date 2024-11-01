namespace Library.Services;
using System.Threading.Tasks;
using Library.Data.Repo;
using Library.Models.Entities;

public class ExternalSourceService
{

    private readonly ProductRepository _productRepository;
    private readonly UserRepository _userRepository;
    private readonly ManyToManyService _manyToManyService;
    private readonly Random _random = new Random();
    
    // Collections for Random Data
    private readonly List<string> _bookAuthors = new() { "J.K. Rowling", "George R.R. Martin", "J.R.R. Tolkien", "Stephen King", "Agatha Christie" };
    private readonly List<string> _musicArtists = new() { "The Beatles", "Elvis Presley", "Madonna", "Taylor Swift", "Adele", "Beyoncé" };
    private readonly List<string> _filmDirectors = new() { "Steven Spielberg", "Quentin Tarantino", "Christopher Nolan", "James Cameron", "Martin Scorsese" };
    private readonly List<string> _movieActors = new() { "Leonardo DiCaprio", "Meryl Streep", "Robert De Niro", "Tom Hanks", "Scarlett Johansson" };
    private readonly List<string> _gameDevelopers = new() { "Nintendo", "Ubisoft", "Rockstar Games", "EA Games", "Valve" };
    private readonly List<string> _gamePublishers = new() { "Nintendo", "Ubisoft", "Rockstar Games", "EA Games", "Valve"};
    private readonly List<string> _titles = new() { "Harry Potter", "Bohemian Rhapsody", "Inception", "The Godfather", "Super Mario Bros", "The Witcher", "Interstellar", "Halo", "Cyberpunk 2077" };
    private readonly List<string> _bookGenres = new() { "Fiction", "Fantasy", "Mystery", "Science Fiction", "Romance" };
    private readonly List<string> _musicGenres = new() { "Pop", "Rock", "Jazz", "Classical", "Hip Hop" };
    private readonly List<string> _movieGenres = new() { "Action", "Drama", "Comedy", "Thriller", "Horror" };
    private readonly List<string> _gameGenres = new() { "Adventure", "RPG", "Shooter", "Puzzle", "Strategy" };

    public ExternalSourceService(ProductRepository productRepository
                                ,UserRepository userRepository
                                ,ManyToManyService manyToManyService)
    {
        _productRepository = productRepository;
        _userRepository = userRepository;
        _manyToManyService = manyToManyService;
    }

    public async Task<Product> GetProduct()
    {
        var productType = (ProductType)_random.Next(1, Enum.GetValues(typeof(ProductType)).Length + 1);
        var product = GenerateRandomProduct(productType);
        switch (productType)
        {
            case ProductType.Book:
                product.Book = await GetRandomBook(); 
                break;
            case ProductType.Disc:
                product.Disc = await GetRandomDisc();
                break;
            default:
                throw new Exception("Invalid product type");
        }
        return product;
    }

    private Product GenerateRandomProduct(ProductType productType)
    {
        return new Product
        {
            Title = _titles[_random.Next(_titles.Count)],
            ProductType = productType,
            Owner = _userRepository.GetOwnerOfLibrary()
        };
    }


    public async Task<Book> GetRandomBook()
    {
        var book = new Book
        {
            BookId = _random.Next(1, 1000),
            Product = GenerateRandomProduct(ProductType.Book),
            Author = _bookAuthors[_random.Next(_bookAuthors.Count)],
            PublicationYear = _random.Next(1950, DateTime.Now.Year + 1),
            Genre = await _manyToManyService.ProcessBookGenresAsync(_bookGenres[_random.Next(_bookGenres.Count)])
        };

        return book;
    }

    public async Task<Disc> GetRandomDisc()
    {
        var discType = (DiscType)_random.Next(1, Enum.GetValues(typeof(DiscType)).Length + 1);
        Movie movie = null;
        Music music = null; 
        Game game = null;

        switch (discType)
        {
            case DiscType.Movie:
                movie = await GenerateRandomMovie();
                break;
            case DiscType.Music:
                music = await GenerateRandomMusic();
                break;
            case DiscType.Game:
                game = await GenerateRandomGame();
                break;
            default:
                throw new Exception("Invalid disc type: " + ((int)discType).ToString());
        }

        var disc = new Disc
        {
            Product = GenerateRandomProduct(ProductType.Disc),
            Format = (DiscFormat)_random.Next(1, Enum.GetValues(typeof(DiscFormat)).Length + 1),
            Year = _random.Next(1990, DateTime.Now.Year + 1),
            DiscType = discType,
            Movie = movie,
            Music = music,
            Game = game
        };

        return disc;
    }

    private async Task<Movie> GenerateRandomMovie()
    {
        return new Movie
        {
            MovieId = _random.Next(1, 1000),
            Duration = _random.Next(60, 180),
            Director = _filmDirectors[_random.Next(_filmDirectors.Count)],
            Genre = await _manyToManyService.ProcessMovieGenresAsync(_movieGenres[_random.Next(_movieGenres.Count)]),
            Actors = await _manyToManyService.ProcessActorsAsync(_movieActors[_random.Next(_movieActors.Count)])
        };
    }

    private async Task<Music> GenerateRandomMusic()
    {
        return new Music
        {
            MusicId = _random.Next(1, 1000),
            Artist = _musicArtists[_random.Next(_musicArtists.Count)],
            TrackCount = _random.Next(1, 20),
            Genre = await _manyToManyService.ProcessMusicGenresAsync(_musicGenres[_random.Next(_musicGenres.Count)]),
        };
    }

    // Generate Random Game
    private async Task<Game> GenerateRandomGame()
    {
        return new Game
        {
            GameId = _random.Next(1, 1000),
            Developer = _gameDevelopers[_random.Next(_gameDevelopers.Count)],
            Publisher = _gamePublishers[_random.Next(_gamePublishers.Count)],
            Genre = await _manyToManyService.ProcessGameGenresAsync(_gameGenres[_random.Next(_gameGenres.Count)]),
        };
    }
}
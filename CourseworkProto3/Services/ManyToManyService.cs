using Library.Data;
using Library.Models.DTO;
using Library.Models.Entities;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
namespace Library.Services;

public class ManyToManyService
{
    private readonly LibraryContext _context;

    public ManyToManyService(LibraryContext context)
    {
        _context = context;
    }

    public async Task<List<BookGenre>> ProcessBookGenresAsync(string input)
    {
        return await ProcessGenresAsync<BookGenre>(input, _context.BookGenres);
    }

    public async Task<List<GameGenre>> ProcessGameGenresAsync(string input)
    {
        return await ProcessGenresAsync<GameGenre>(input, _context.GameGenres);
    }

    public async Task<List<MusicGenre>> ProcessMusicGenresAsync(string input)
    {
        return await ProcessGenresAsync<MusicGenre>(input, _context.MusicGenres);
    }

    public async Task<List<MovieGenre>> ProcessMovieGenresAsync(string input)
    {
        return await ProcessGenresAsync<MovieGenre>(input, _context.MovieGenres);
    }

    public async Task<List<TGenre>> ProcessGenresAsync<TGenre>(string input, DbSet<TGenre> genreSet) where TGenre : Genre, new()
    {
        var genreNames = input.Split(',')
                            .Select(g => g.Trim())
                            .Where(g => !string.IsNullOrWhiteSpace(g))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

        var genresToAdd = new List<TGenre>();
        var existingGenres = await genreSet.ToListAsync();
        var resultGenres = new List<TGenre>();

        foreach (var genreName in genreNames)
        {
            var existingGenre = existingGenres
                .FirstOrDefault(g => g.Name.Equals(genreName, StringComparison.OrdinalIgnoreCase));

            if (existingGenre != null)
            {
                resultGenres.Add(existingGenre);
            }
            else
            {
                var newGenre = new TGenre { Name = genreName };
                genresToAdd.Add(newGenre);
                resultGenres.Add(newGenre);
            }
        }

        if (genresToAdd.Any())
        {
            genreSet.AddRange(genresToAdd);
            await _context.SaveChangesAsync();
        }

        return resultGenres;
    }

    public async Task<List<Actor>> ProcessActorsAsync(string input)
    {
        var actorNames = input.Split(',')
                            .Select(a => a.Trim())
                            .Where(a => !string.IsNullOrWhiteSpace(a))
                            .Distinct(StringComparer.OrdinalIgnoreCase)
                            .ToList();

        var actorsToAdd = new List<Actor>();
        var existingActors = await _context.Actors.ToListAsync();
        var resultActors = new List<Actor>();

        foreach (var actorName in actorNames)
        {
            var existingActor = existingActors
                .FirstOrDefault(a => a.Name.Equals(actorName, StringComparison.OrdinalIgnoreCase));

            if (existingActor != null)
            {
                resultActors.Add(existingActor);
            }
            else
            {
                var newActor = new Actor { Name = actorName };
                actorsToAdd.Add(newActor);
                resultActors.Add(newActor);
            }
        }

        if (actorsToAdd.Any())
        {
            _context.Actors.AddRange(actorsToAdd);
            await _context.SaveChangesAsync();
        }

        return resultActors;
    }

    public async Task<Tuple<List<BookGenre>, List<MusicGenre>,List<MovieGenre>, List<GameGenre>, List<Actor>>> GetGenresAndActors(){
        List<BookGenre> bookGenres = await _context.BookGenres
                                                    .Include(b => b.Books)
                                                        .ThenInclude(b => b.Product)
                                                    .AsNoTracking().ToListAsync();

        List<MusicGenre> musicGenres = await _context.MusicGenres
                                                    .Include(b => b.Music)
                                                        .ThenInclude(m => m.Disc)
                                                            .ThenInclude(d => d.Product)
                                                    .AsNoTracking().ToListAsync();

        List<MovieGenre> movieGenres = await _context.MovieGenres
                                                    .Include(b => b.Movies)
                                                        .ThenInclude(m => m.Disc)
                                                            .ThenInclude(d => d.Product)
                                                    .AsNoTracking().ToListAsync();     

        List<GameGenre> gameGenres = await _context.GameGenres
                                                    .Include(b => b.Games)
                                                        .ThenInclude(m => m.Disc)
                                                            .ThenInclude(d => d.Product)
                                                    .AsNoTracking().ToListAsync();

        List<Actor> actors = await _context.Actors
                                            .Include(b => b.Movies)
                                                .ThenInclude(m => m.Disc)
                                                    .ThenInclude(d => d.Product)
                                            .AsNoTracking().ToListAsync();

        return Tuple.Create(bookGenres, musicGenres, movieGenres, gameGenres, actors);
    } 

    public async Task Update(EditActorOrGenreRequest request){
        switch (request.Type){
            case "actor":
                var actor = await _context.Actors.FirstOrDefaultAsync(a => a.ActorId == request.Id);
                actor!.Name = request.Name;
                _context.SaveChanges();
                break;
            case "book":
                var book = await _context.BookGenres.FirstOrDefaultAsync(a => a.GenreId == request.Id);
                book!.Name = request.Name;
                _context.SaveChanges();
                break;
            case "movie":
                var movie = await _context.MovieGenres.FirstOrDefaultAsync(a => a.GenreId == request.Id);
                movie!.Name = request.Name;
                _context.SaveChanges();
                break;
            case "music":
                var music = await _context.MusicGenres.FirstOrDefaultAsync(a => a.GenreId == request.Id);
                music!.Name = request.Name;
                _context.SaveChanges();
                break;
            case "game":
                var game = await _context.GameGenres.FirstOrDefaultAsync(a => a.GenreId == request.Id);
                game!.Name = request.Name; 
                _context.SaveChanges();
                break;
            default: break;
        }
    }

    public async Task Delete(int id, string type){
        switch (type){
            case "actor":
                var actor = await _context.Actors.FirstOrDefaultAsync(a => a.ActorId == id);
                if (actor != null){
                    _context.Actors.Remove(actor);
                    _context.SaveChanges();
                }
                break;
            case "book":
                var book = await _context.BookGenres.FirstOrDefaultAsync(a => a.GenreId == id);
                if(book != null){
                    _context.BookGenres.Remove(book!);
                    _context.SaveChanges();
                }
                break;
            case "movie":
                var movie = await _context.MovieGenres.FirstOrDefaultAsync(a => a.GenreId == id);
                if(movie != null){
                    _context.MovieGenres.Remove(movie!);
                    _context.SaveChanges();
                }
                break;
            case "music":
                var music = await _context.MusicGenres.FirstOrDefaultAsync(a => a.GenreId == id);
                if(music != null){
                    _context.MusicGenres.Remove(music!);
                    _context.SaveChanges();
                }
                break; ;
            case "game": {}
                var game = await _context.GameGenres.FirstOrDefaultAsync(a => a.GenreId == id);
                if(game != null){
                    _context.GameGenres.Remove(game!);
                    _context.SaveChanges(); 
                }
                break;
            default: break;
        }
    }

    public async Task<bool> ActorOrGenreExists(int id){
        return await _context.BookGenres.AnyAsync(b => b.GenreId == id) || 
                await _context.MusicGenres.AnyAsync(m => m.GenreId == id) || 
                await _context.MovieGenres.AnyAsync(m => m.GenreId == id) || 
                await _context.GameGenres.AnyAsync(g => g.GenreId == id) ||
                await _context.Actors.AnyAsync(a => a.ActorId == id); 
    }

}

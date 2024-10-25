using Library.Data;
using Library.Models.Entities;
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
}

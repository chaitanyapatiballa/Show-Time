using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic;

public class MovieLogic
{
    private readonly AppDbContext _context;
    private readonly MovieRepository _repository;

    public MovieLogic(AppDbContext context)
    {
        _context = context;
        _repository = new MovieRepository(context);
    }

    // Movie
    public async Task<List<Movie>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Movie?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);

    public async Task<Movie> AddAsync(MovieDto dto)
    {
        var movie = new Movie
        {
            Title = dto.Title,
            Duration = dto.Duration,
            Genre = dto.Genre,
            releasedate = dto.releasedate
        };

        return await _repository.AddAsync(movie);
    }
    public async Task UpdateAsync(int id, MovieDto dto)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie == null) return;

        movie.Title = dto.Title;
        movie.Duration = dto.Duration;
        movie.Genre = dto.Genre;
        movie.releasedate = dto.releasedate;

        await _repository.UpdateAsync(movie);
    }
    public async Task DeleteAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);
        if (movie != null)
        {
            await _repository.DeleteAsync(movie);
        }
    }


    // Showtemplate
    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() =>
        await _repository.GetAllShowtemplatesAsync();

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) =>
        await _repository.GetShowtemplateByIdAsync(id);

    public async Task<Showtemplate> AddShowtemplateAsync(ShowtemplateDto dto)
    {
        var template = new Showtemplate
        {
            Baseprice = dto.Baseprice,
            Format = dto.Format,
            Language = dto.Language,
            Movieid = dto.Movieid,
            Theaterid = dto.Theaterid
        };

        bool exists = await _context.MovieTheaters.AnyAsync(mt =>
            mt.Movieid == dto.Movieid && mt.Theaterid == dto.Theaterid);

        if (!exists)
        {
            _context.MovieTheaters.Add(new MovieTheater
            {
                Movieid = dto.Movieid,
                Theaterid = dto.Theaterid
            });
        }

        _context.Showtemplates.Add(template);
        await _context.SaveChangesAsync();

        return template;
    }

    public async Task UpdateShowtemplateAsync(int id, ShowtemplateDto dto)
    {
        var existing = await _context.Showtemplates.FindAsync(id);
        if (existing == null) return;

        existing.Baseprice = dto.Baseprice;
        existing.Format = dto.Format;
        existing.Language = dto.Language;
        existing.Movieid = dto.Movieid;
        existing.Theaterid = dto.Theaterid;

        _context.Showtemplates.Update(existing);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteShowtemplateAsync(int id)
    {
        var existing = await _repository.GetShowtemplateByIdAsync(id);
        if (existing != null)
        {
            await _repository.DeleteShowtemplateAsync(existing);
        }
    }

    // Showinstance
    public async Task<List<Showinstance>> GetAllShowinstancesAsync() =>
        await _repository.GetAllShowinstancesAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) =>
        await _repository.GetShowinstanceByIdAsync(id);

    public async Task AddShowinstanceAsync(Showinstance instance) =>
        await _repository.AddShowinstanceAsync(instance);

    public async Task UpdateShowinstanceAsync(Showinstance instance) =>
        await _repository.UpdateShowinstanceAsync(instance);

    public async Task DeleteShowinstanceAsync(Showinstance instance) =>
        await _repository.DeleteShowinstanceAsync(instance);
}

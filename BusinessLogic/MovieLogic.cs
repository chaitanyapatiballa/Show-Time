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

    public async Task<Movie> AddAsync(Movie movie)
    {
        return await _repository.AddMovieAsync(movie);
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
            mt.movieid == dto.Movieid && mt.theaterid == dto.Theaterid);

        if (!exists)
        {
            _context.MovieTheaters.Add(new MovieTheater
            {
                movieid = dto.Movieid,
                theaterid = dto.Theaterid
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

    // Showinstance - Business Logic Layer
    public async Task<List<Showinstance>> GetAllShowinstancesAsync() =>
        await _repository.GetAllShowinstancesAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) =>
        await _repository.GetShowinstanceByIdAsync(id);

    public async Task<Showinstance> AddShowinstanceAsync(ShowinstanceDto dto)
    {
        var showtemplate = await _context.Showtemplates.FindAsync(dto.Showtemplateid);
        if (showtemplate == null)
            throw new Exception("Invalid Showtemplate ID");

        var instance = new Showinstance
        {
            Availableseats = dto.Availableseats,
            Showdate = dto.Showdate,
            Showtime = dto.Showtime,
            Showtemplateid = dto.Showtemplateid
        };

        await _repository.AddShowinstanceAsync(instance);
        return instance;
    }

    public async Task<bool> UpdateShowinstanceAsync(int id, ShowinstanceDto dto)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        instance.Availableseats = dto.Availableseats;
        instance.Showdate = dto.Showdate;
        instance.Showtime = dto.Showtime;
        instance.Showtemplateid = dto.Showtemplateid;

        await _repository.UpdateShowinstanceAsync(instance);
        return true;
    }

    public async Task<bool> DeleteShowinstanceAsync(int id)
    {
        var instance = await _repository.GetShowinstanceByIdAsync(id);
        if (instance == null) return false;

        await _repository.DeleteShowinstanceAsync(instance);
        return true;
    }

}

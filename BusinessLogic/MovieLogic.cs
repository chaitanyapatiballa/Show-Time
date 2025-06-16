using DataAccessLayer.Repositories;
using DBModels.Dto;
using DBModels.Models;

namespace BusinessLogic;

public class MovieLogic(MovieRepository repository)
{
    private readonly MovieRepository _repository = repository;

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

    public async Task UpdateAsync(Movie movie) => await _repository.UpdateAsync(movie);

    public async Task DeleteAsync(Movie movie) => await _repository.DeleteAsync(movie);

    // ShowTemplate
    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() => await _repository.GetAllShowtemplatesAsync();

    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) => await _repository.GetShowtemplateByIdAsync(id);

    public async Task AddShowtemplateAsync(Showtemplate template) => await _repository.AddShowtemplateAsync(template);

    public async Task UpdateShowtemplateAsync(Showtemplate template) => await _repository.UpdateShowtemplateAsync(template);

    public async Task DeleteShowtemplateAsync(Showtemplate template) => await _repository.DeleteShowtemplateAsync(template);

    // ShowInstance
    public async Task<List<Showinstance>> GetAllShowinstancesAsync() => await _repository.GetAllShowinstancesAsync();

    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) => await _repository.GetShowinstanceByIdAsync(id);

    public async Task AddShowinstanceAsync(Showinstance instance) => await _repository.AddShowinstanceAsync(instance);

    public async Task UpdateShowinstanceAsync(Showinstance instance) => await _repository.UpdateShowinstanceAsync(instance);

    public async Task DeleteShowinstanceAsync(Showinstance instance) => await _repository.DeleteShowinstanceAsync(instance);
}

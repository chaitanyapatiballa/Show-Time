using DBModels.Models;
using MovieService.Repositories;

namespace MovieService.Services;

public class IMovieService
{
    private readonly MovieRepository _repository;

    public IMovieService(MovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Movie>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Movie?> GetByIdAsync(int id) => await _repository.GetByIdAsync(id);
    public async Task AddAsync(Movie movie) => await _repository.AddAsync(movie);
    public async Task UpdateAsync(Movie movie) => await _repository.UpdateAsync(movie);
    public async Task DeleteAsync(Movie movie) => await _repository.DeleteAsync(movie);

    public async Task<List<Showtemplate>> GetAllShowtemplatesAsync() => await _repository.GetAllShowtemplatesAsync();
    public async Task<Showtemplate?> GetShowtemplateByIdAsync(int id) => await _repository.GetShowtemplateByIdAsync(id);
    public async Task AddShowtemplateAsync(Showtemplate template) => await _repository.AddShowtemplateAsync(template);
    public async Task UpdateShowtemplateAsync(Showtemplate template) => await _repository.UpdateShowtemplateAsync(template);
    public async Task DeleteShowtemplateAsync(Showtemplate template) => await _repository.DeleteShowtemplateAsync(template);

    public async Task<List<Showinstance>> GetAllShowinstancesAsync() => await _repository.GetAllShowinstancesAsync();
    public async Task<Showinstance?> GetShowinstanceByIdAsync(int id) => await _repository.GetShowinstanceByIdAsync(id);
    public async Task AddShowinstanceAsync(Showinstance instance) => await _repository.AddShowinstanceAsync(instance);
    public async Task UpdateShowinstanceAsync(Showinstance instance) => await _repository.UpdateShowinstanceAsync(instance);
    public async Task DeleteShowinstanceAsync(Showinstance instance) => await _repository.DeleteShowinstanceAsync(instance);
}

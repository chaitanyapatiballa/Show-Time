using DBModels.Db;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Booking_Service.HttpClients
{
    public class MovieServiceClient : IMovieServiceClient
    {
        private readonly HttpClient _httpClient;

        public MovieServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Movie> GetMovieAsync(int movieId)
        {
            var response = await _httpClient.GetAsync($"/api/movie/{movieId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Movie>();
        }
    }
}

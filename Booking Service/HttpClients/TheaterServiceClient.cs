using DBModels.Db;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Booking_Service.HttpClients
{
    public class TheaterServiceClient : ITheaterServiceClient
    {
        private readonly HttpClient _httpClient;

        public TheaterServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Theater> GetTheaterAsync(int theaterId)
        {
            var response = await _httpClient.GetAsync($"/api/theater/{theaterId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Theater>();
        }
    }
}

using DataAccessLayer.Repositories;
using DBModels.Models;

namespace BusinessLogic
{
    public class ShowManager
    {
        private readonly ShowRepository _repository;

        public ShowManager(ShowRepository repository)
        {
            _repository = repository;
        }

        public async Task<Showinstance?> GetShowInstance(int movieId, int theaterId, DateTime showTime)
        {
            return await _repository.GetShowInstance(movieId, theaterId, showTime);
        }

        public async Task<Showtemplate> CreateTemplate(int movieId, int theaterId)
        {
            var template = new Showtemplate
            {
                Movieid = movieId,
                Theaterid = theaterId
            };
            return await _repository.AddTemplateAsync(template);
        }

        public async Task<Showinstance> CreateInstance(int templateId, DateTime showTime, decimal ticketPrice)
        {
            var instance = new Showinstance
            {
                Showtemplateid = templateId,
                Showdate = DateOnly.FromDateTime(showTime), 
                Showtime = TimeOnly.FromDateTime(showTime), 
                ShowTime = showTime, 
                TicketPrice = ticketPrice
            };
            return await _repository.AddInstanceAsync(instance);
        }
    }
}

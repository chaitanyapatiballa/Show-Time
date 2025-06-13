using DataAccessLayer.Repositories;
using DBModels.Db;

namespace BusinessLogic
{
    public class ShowManager
    {
        private readonly ShowRepository _repository;

        public ShowManager(ShowRepository repository)
        {
            _repository = repository;
        }

        public async Task<ShowInstance?> GetShowInstance(int movieId, int theaterId, DateTime showTime)
        {
            return await _repository.GetShowInstance(movieId, theaterId, showTime);
        }

        public async Task<ShowTemplate> CreateTemplate(int movieId, int theaterId)
        {
            var template = new ShowTemplate { MovieId = movieId, TheaterId = theaterId };
            return await _repository.AddTemplateAsync(template);
        }

        public async Task<ShowInstance> CreateInstance(int templateId, DateTime showTime, decimal ticketPrice)
        {
            var instance = new ShowInstance
            {
                ShowTemplateId = templateId,
                ShowTime = showTime,
                TicketPrice = ticketPrice
            };
            return await _repository.AddInstanceAsync(instance);
        }
    }
}


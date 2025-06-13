using BusinessLogic;
using DBModels.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ShowService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShowsController : ControllerBase
    {
        private readonly ShowManager _manager;

        public ShowsController(ShowManager manager)
        {
            _manager = manager;
        }

        [HttpGet("GetShow")]
        public async Task<ActionResult<ShowInstanceDto>> GetShow(int movieId, int theaterId, DateTime showTime)
        {
            var show = await _manager.GetShowInstance(movieId, theaterId, showTime);
            if (show == null) return NotFound();

            return new ShowInstanceDto
            {
                ShowInstanceId = show.ShowInstanceId,
                ShowTemplateId = show.ShowTemplateId,
                ShowTime = show.ShowTime,
                TicketPrice = show.TicketPrice
            };
        }

        [HttpPost("CreateTemplate")]
        public async Task<ActionResult<ShowTemplateDto>> CreateTemplate([FromBody] ShowTemplateDto dto)
        {
            var template = await _manager.CreateTemplate(dto.MovieId, dto.TheaterId);
            return new ShowTemplateDto
            {
                ShowTemplateId = template.ShowTemplateId,
                MovieId = template.MovieId,
                TheaterId = template.TheaterId
            };
        }

        [HttpPost("CreateInstance")]
        public async Task<ActionResult<ShowInstanceDto>> CreateInstance([FromBody] ShowInstanceDto dto)
        {
            var instance = await _manager.CreateInstance(dto.ShowTemplateId, dto.ShowTime, dto.TicketPrice);
            return new ShowInstanceDto
            {
                ShowInstanceId = instance.ShowInstanceId,
                ShowTemplateId = instance.ShowTemplateId,
                ShowTime = instance.ShowTime,
                TicketPrice = instance.TicketPrice
            };
        }
    }
}

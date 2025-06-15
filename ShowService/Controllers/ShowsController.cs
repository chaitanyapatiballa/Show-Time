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
                ShowInstanceid = show.Showinstanceid,
                ShowTemplateid = show.Showtemplateid,
                ShowTime = show.ShowTime,
                TicketPrice = show.TicketPrice
            };
        }

        [HttpPost("CreateInstance")]
        public async Task<ActionResult<ShowInstanceDto>> CreateInstance([FromBody] ShowInstanceDto dto)
        {
            if (dto.ShowTemplateid == null)
                return BadRequest("ShowTemplateid is required.");

            var instance = await _manager.CreateInstance(dto.ShowTemplateid.Value, dto.ShowTime, dto.TicketPrice);
            return new ShowInstanceDto
            {
                ShowInstanceid = instance.Showinstanceid,
                ShowTemplateid = instance.Showtemplateid,
                ShowTime = instance.ShowTime,
                TicketPrice = instance.TicketPrice
            };
        }
    }
}

        
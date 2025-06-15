using DBModels.Models;
namespace DBModels.Dto
{
    public class ShowInstanceDto
    {
        public int? ShowTemplateid { get; set; }
        public DateTime ShowTime { get; set; }
        public decimal TicketPrice { get; set; }
        public int ShowInstanceid { get; set; }
    }
}
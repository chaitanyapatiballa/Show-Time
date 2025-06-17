using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.Dto
{
    public class ShowinstanceDto
    {
        public int Showinstanceid { get; set; }
        public int Availableseats { get; set; }
        public DateOnly Showdate { get; set; }
        public TimeOnly Showtime { get; set; }
        public int Showtemplateid { get; set; }
    }
}
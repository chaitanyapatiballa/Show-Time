using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.Dto
{
    public class SeatStatusDto
    {
        public int Seatid { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Number { get; set; }
        public bool Isbooked { get; set; }
        public decimal Price { get; set; }
    }
}


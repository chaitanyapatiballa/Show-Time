using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.Dto
{
    public class ShowtemplateDto
    {
        public int Showtemplateid { get; set; }
        public decimal Baseprice { get; set; }
        public string Format { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public int Movieid { get; set; }
        public int Theaterid { get; set; }
    }
}

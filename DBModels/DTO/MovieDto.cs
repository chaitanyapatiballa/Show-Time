using DBModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.Dto
{
    public class MovieDto
    {

        public string Title { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime? releasedate { get; set; }

        public ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();

    }
}
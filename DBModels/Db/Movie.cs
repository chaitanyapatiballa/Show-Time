using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBModels.Db
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Genre { get; set; }
       
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();
        
    }
}

using System.Collections.Generic;

namespace DBModels.Db
{
    public partial class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string Duration { get; set; } = null!;
        public int TheaterId { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();
    }
}

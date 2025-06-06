
namespace DBModels.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public int Duration { get; set; }
        public ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();
    }
}
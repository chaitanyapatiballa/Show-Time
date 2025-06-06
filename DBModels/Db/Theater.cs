
namespace DBModels.Models
{
    public class Theater
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }
        public int Capacity { get; set; }
        public ICollection<MovieTheater> MovieTheaters { get; set; } = new List<MovieTheater>();
    }
}
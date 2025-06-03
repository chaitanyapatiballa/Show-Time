namespace MovieService.DTOs
{
    public class MovieDto   
    {

        public  required string Title { get; set; }
        public required string Genre { get; set; }
        public int Duration { get; set; }

    }
}

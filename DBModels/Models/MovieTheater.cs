using System.Text.Json.Serialization;

namespace DBModels.Models
{
    public class Movietheater   
    {
        public int Movieid { get; set; }
        public int Theaterid { get; set; }

        [JsonIgnore]
        public Movie? Movie { get; set; }

        [JsonIgnore]
        public Theater? Theater { get; set; }
    }
}
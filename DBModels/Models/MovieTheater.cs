

namespace DBModels.Models
{
    public partial class Movietheater   
    {
        public int Movieid { get; set; }
        public int Theaterid { get; set; }

        //[JsonIgnore]
        //public Movie? Movie { get; set; }

        //[JsonIgnore]
        //public Theater? Theater { get; set; }
    }
}
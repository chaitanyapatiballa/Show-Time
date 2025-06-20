using DBModels.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DBModels.Models
{
    public partial class Movie
    {
        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; }

        [JsonIgnore]
        public ICollection<Showtemplate> Showtemplates { get; set; }

        [JsonIgnore]
        public ICollection<Movietheater> Movietheater { get; set; } = new List<Movietheater>();

        public virtual ICollection<Theater> Theaters { get; set; } = new List<Theater>();
    }

    public partial class Theater
    {
        [JsonIgnore]
        public ICollection<Movietheater> Movietheater { get; set; } = new List<Movietheater>();

        [JsonIgnore]
        public ICollection<Booking> Bookings { get; set; }

        [JsonIgnore]
        public ICollection<Showtemplate> Showtemplates { get; set; }

        [JsonIgnore]
        public ICollection<Seat> Seats { get; set; }
        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }

    //public partial class Booking
    //{
    //    [JsonIgnore]
    //    public Movie? Movie { get; set; }

    //    [JsonIgnore]
    //    public Theater? Theater { get; set; }

    //    [JsonIgnore]
    //    public Payment? Payment { get; set; }

    //    [JsonIgnore]
    //    public ICollection<Billingsummary> Billingsummaries { get; set; }
    //}

    //public partial class Showtemplate
    //{
    //    [JsonIgnore]
    //    public Movie? Movie { get; set; }

    //    [JsonIgnore]
    //    public Theater? Theater { get; set; }

    //    [JsonIgnore]
    //    public ICollection<Showinstance> Showinstances { get; set; }
    //}

    public partial class Showinstance
    {
        [JsonIgnore]
        public Showtemplate? Showtemplate { get; set; }

        [JsonIgnore]
        public ICollection<Showseatstatus> Showseatstatuses { get; set; }
    }

    //public partial class Showseatstatus
    //{
    //    [JsonIgnore]
    //    public Seat Seat { get; set; }

    //    [JsonIgnore]
    //    public Showinstance Showinstance { get; set; }
    //}

    public partial class Seat
    {
        [JsonIgnore]
        public Theater Theater { get; set; }

        [JsonIgnore]
        public ICollection<Showseatstatus> Showseatstatuses { get; set; }
    }

    //public partial class Payment
    //{
    //    [ForeignKey("Bookingid")]
    //    public virtual Booking? Booking { get; set; }
    //}

    //public partial class Billingsummary
    //{
    //    [JsonIgnore]
    //    public Booking? Booking { get; set; }
    //}

    public partial class Movietheater
    {
        [JsonIgnore]
        public Movie? Movie { get; set; }

        [JsonIgnore]
        public Theater? Theater { get; set; }
    }
}

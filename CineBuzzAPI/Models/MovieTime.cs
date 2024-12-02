namespace CineBuzzApi.Models
{

    /// Represents a showtime for a movie.

    public class MovieTime
    {

        /// Unique identifier for each showtime.

        public int MovieTimeId { get; set; }


        /// The ID of the movie being shown.

        public int MovieId { get; set; }


        /// The date and time when the movie is scheduled to be shown.

        public DateTime MovieDateTime { get; set; }


        /// The location or theater where the movie will be shown.

        public string Location { get; set; } = string.Empty;


        /// Navigation property for the associated Movie.

        public Movie Movie { get; set; }  // Navigation property to Movie


        /// Collection of tickets associated with this showtime.

        public List<Ticket> Tickets { get; set; } = new List<Ticket>();


        /// Default constructor for ShowTime.

        public MovieTime() { }
    }
}
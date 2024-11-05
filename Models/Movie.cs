using System.Text.Json.Serialization;
namespace MovieReviewApi.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string Genre { get; set; }
        public int Rating { get; set; } // Rating from 1 to 5
        [JsonIgnore]
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();  // Navigation property
    }
}

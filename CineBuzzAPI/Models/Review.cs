namespace CineBuzzApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty; // Review content
         // Navigation properties
        public Movie Movie { get; set; }  // Navigation property to the Movie entity
        public User User { get; set; }    // Assuming you might also have a User entity for the reviewer
    }
}

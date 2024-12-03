using System.ComponentModel.DataAnnotations;

namespace CineBuzzApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty; // Review content

        // Do a number system for stars
    }
}

using System;
using System.Collections.Generic;

namespace CineBuzzApi.Models
{
    public class Movie
    {
        // Unique identifier for the movie
        public int MovieId { get; set; }

        // Title of the movie, initialized to a non-null default value
        public string Title { get; set; } = string.Empty;

        // A short description or synopsis of the movie, initialized to a non-null default value
        public string Description { get; set; } = string.Empty;

        // A list of genres associated with the movie, using List collection
        public List<string> Genres { get; set; }

        // Constructor to initialize the Genres list
        public Movie()
        {
            Genres = new List<string>();
        }
        public override bool Equals(object obj)
        {
            if (obj is Movie other)
            {
                return MovieId == other.MovieId;
            }
            return false;
        }
    }
}

/*
 * Copyright 2024 USC - Columbia
 * @author Scott Do
 * @date 11/18/2024
 *  Review model.
 */

using System.ComponentModel.DataAnnotations;

namespace CineBuzzApi.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; } = string.Empty; // Review content
    }
}

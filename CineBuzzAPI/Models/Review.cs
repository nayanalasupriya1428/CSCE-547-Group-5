/*
 * Copyright 2024 USC - Columbia
 * @author Scott Do
 * @date 11/18/2024
 *  Review model.
 */

using System.ComponentModel.DataAnnotations;

namespace CineBuzzApi.Models
{
    /// <summary>
    /// Review entity.
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Identifier for the review.
        /// </summary>
        public int ReviewID { get; set; }

        /// <summary>
        /// ID of the movie being reviewed.
        /// </summary>
        public int MovieID { get; set; }

        /// <summary>
        /// ID of the user who wrote the review.
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// The content of the review.
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// The star rating of the review.
        /// </summary>
        [Range(0, 5)]
        public int ReviewScore { get; set; }

        /// <summary>
        /// The date when the review was made.
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReviewDate { get; set; }

    }
}
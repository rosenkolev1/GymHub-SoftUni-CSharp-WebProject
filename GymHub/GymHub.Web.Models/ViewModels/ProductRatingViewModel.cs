using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductRatingViewModel
    {
        public ProductRatingViewModel()
        {

        }
        public ProductRatingViewModel(double averageRating)
        {
            this.AverageRating = averageRating;
        }

        [Range(1, 10, ErrorMessage = "Rating should be between 1 and 10.")]
        public double? AverageRating { get; set; }

        public int FullStarsCount => this.AverageRating != null ? (int)Math.Floor((double)AverageRating) : 0;
        public int EmptyStarsCount => this.AverageRating != null ? 10 - (int)Math.Ceiling((double)AverageRating) : 0;

        public bool HasHalfStar => 10 - (this.FullStarsCount + this.EmptyStarsCount) > 0;
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductRatingViewModel
    {
        public ProductRatingViewModel(double averageRating)
        {
            this.AverageRating = averageRating;
        }
        public double AverageRating { get; set; }
        public int FullStarsCount => (int)Math.Floor(AverageRating);
        public int EmptyStarsCount => 10 - (int)Math.Ceiling(AverageRating);
        public bool HasHalfStar => 10 - (this.FullStarsCount + this.EmptyStarsCount) > 0;
    }
}

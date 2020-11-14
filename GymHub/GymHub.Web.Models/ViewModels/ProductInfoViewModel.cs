using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GymHub.Web.Models.ViewModels
{
    public class ProductInfoViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        //Warranty in days
        public int Warranty { get; set; }
        public int QuantityInStock { get; set; }
        public string Model { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<ProductRating> ProductRatings { get; set; }

        public string ShortDescription { get; set; }

        public double AverageRating { get; set; }

        public int FullStarsCount => (int)Math.Floor(AverageRating);
        public int EmptyStarsCount => 10 - (int)Math.Ceiling(AverageRating);
        public bool HasHalfStar => 10 - (this.FullStarsCount + this.EmptyStarsCount) > 0;
    }
}

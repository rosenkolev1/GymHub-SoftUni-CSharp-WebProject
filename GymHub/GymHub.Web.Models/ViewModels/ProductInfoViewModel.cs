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
        public string ShortDescription => this.GetShortDescription();
        public string Model { get; set; }
        public List<ProductComment> ProductComments { get; set; }
        public List<ProductRating> ProductRatings { get; set; }

        private string GetShortDescription()
        {
            var returnString = new StringBuilder();
            var descriptionWords = Description.Split(" ").ToList();
            foreach (var word in descriptionWords)
            {
                returnString.Append(word);
                if (returnString.ToString().Length >= 40) break;
                returnString.Append(" ");
            }
            returnString.Append("...");
            return returnString.ToString();
        }
    }
}

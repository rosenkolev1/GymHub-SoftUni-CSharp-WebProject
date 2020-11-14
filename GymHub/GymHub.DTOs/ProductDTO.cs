using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }

        public string Model { get; set; }

        public string MainImage { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Warranty { get; set; }

        public int QuantityInStock { get; set; }

        public List<ProductRatingDTO> ProductRatings { get; set; }
    }
}

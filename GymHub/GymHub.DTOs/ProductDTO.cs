using System.Collections.Generic;

namespace GymHub.DTOs
{
    public class ProductDTO
    {
        public string Name { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public int Warranty { get; set; }

        public int QuantityInStock { get; set; }

        public List<string> Categories { get; set; }

    }
}

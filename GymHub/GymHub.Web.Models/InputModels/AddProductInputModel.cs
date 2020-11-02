namespace GymHub.Web.Models.InputModels
{
    public class AddProductInputModel
    {
        public string Name { get; set; }
        public string MainImage { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Warranty { get; set; }
        public int QuantityInStock { get; set; }
    }
}

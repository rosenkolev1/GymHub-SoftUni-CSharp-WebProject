namespace GymHub.Web.Models.ViewModels
{
    public class ProductViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        //Warranty in days
        public int Warranty { get; set; }
        public int QuantityInStock { get; set; }
        public string ShortDescription { get; set; }
        public string Model { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class EditCategoryInputModel
    {
        [Required]
        public string Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}

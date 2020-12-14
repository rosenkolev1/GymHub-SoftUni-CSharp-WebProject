using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class AddCategoryInputModel
    {
        [Required]
        public string Name { get; set; }
    }
}

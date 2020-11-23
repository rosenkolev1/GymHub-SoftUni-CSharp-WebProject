using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class RemoveCommentInputModel
    {
        [Required(ErrorMessage = "Comment is required.")]
        public string RemoveCommentId { get; set; }

        //[Required(ErrorMessage = "Product is required.")]
        //public string ProductId { get; set; }

        public string Justification { get; set; }
    }
}

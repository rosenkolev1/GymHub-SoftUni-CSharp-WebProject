using System.ComponentModel.DataAnnotations;

namespace GymHub.Web.Models.InputModels
{
    public class RemoveCommentInputModel
    {
        [Required(ErrorMessage = "Comment is required.")]
        public string RemoveCommentId { get; set; }

        public string Justification { get; set; }
    }
}

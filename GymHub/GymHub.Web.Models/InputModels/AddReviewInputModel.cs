using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymHub.Web.Models.InputModels
{
    public class AddReviewInputModel
    {
        [Required]
        [Range(1,10, ErrorMessage = "Rating should be between 1 and 10.")]
        public int? Rating { get; set; }
        public int FullStarsCount => (int)Math.Floor((double)Rating);
        public int EmptyStarsCount => 10 - (int)Math.Ceiling((double)Rating);

        [Required]
        public string Text { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public string ProductId { get; set; }
    }
}

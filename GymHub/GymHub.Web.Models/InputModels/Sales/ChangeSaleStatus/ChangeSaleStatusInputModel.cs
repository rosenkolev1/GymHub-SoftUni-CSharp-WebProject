using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.InputModels
{
    public class ChangeSaleStatusInputModel
    {
        [Required]
        public string SaleId { get; set; }

        [Required]
        public string NewSaleStatusId { get; set; }
    }
}

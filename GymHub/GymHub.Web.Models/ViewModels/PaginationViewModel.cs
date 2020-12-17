using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Web.Models.ViewModels
{
    public class PaginationViewModel
    {
        private int cutOffNumber { get; set; }

        public int CurrentPage { get; set; }
        public int NumberOfPages { get; set; }
        public int CutoffNumber { get => cutOffNumber;  set 
            {
                if (value % 2 != 0) value += 1;
                this.cutOffNumber = value;
            }
        }
    }
}

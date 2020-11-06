using System;
using System.Collections.Generic;
using System.Text;

namespace GymHub.Web.Models
{
    public class ComplexModel<InputType, ViewType>
    {
        public InputType InputModel { get; set; }
        public ViewType ViewModel { get; set; }
    }
}

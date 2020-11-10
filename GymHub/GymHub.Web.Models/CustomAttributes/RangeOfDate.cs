using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace GymHub.Web.Models.CustomAttributes
{
    public class RangeOfDate : ValidationAttribute
    {
        public int MinYear { get; set; }

        public RangeOfDate()
        {
            this.MinYear = DateTime.UtcNow.Year - 130;
        }

        public override bool IsValid(object value)
        {
            var currentDate = DateTime.UtcNow;
            var minDate = new DateTime(MinYear, 1, 1);
            this.ErrorMessage = $"Date must be between {minDate.ToUniversalTime():yyyy/MM/dd} and {currentDate.ToUniversalTime():yyyy/MM/dd}.";
            //Check if value is of type DateTime
            var dateTimeValue = new DateTime();
            if (value is DateTime time)
            {
                dateTimeValue = time;
            }
            else
            {
                return false;
            }

            //Check actual validation requirement
            if(dateTimeValue <= currentDate && dateTimeValue >= minDate)
            {
                return true;
            }

            return false;
        }
    }
}

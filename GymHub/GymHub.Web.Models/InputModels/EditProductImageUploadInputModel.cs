using GymHub.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymHub.Web.Models.InputModels
{
    public class EditProductImageUploadInputModel
    {
        [JsonIgnore]
        public IFormFile ImageUpload { get; set; }

        public bool IsBeingModified { get; set; }

        public ImageIdAndPathInputModel ModifiedImage { get; set; }
    }
}

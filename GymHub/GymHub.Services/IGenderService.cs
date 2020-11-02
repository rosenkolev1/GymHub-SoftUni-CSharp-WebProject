using GymHub.Data.Models;
using System.Collections.Generic;

namespace GymHub.Web.Services
{
    public interface IGenderService
    {
        public void Add(string name);
        public string GetGenderIdByName(string name);
        public List<Gender> GetAllGenders();
        public bool GenderExists(string name);
    }
}

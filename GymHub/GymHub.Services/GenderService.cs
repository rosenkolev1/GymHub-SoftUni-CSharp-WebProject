using GymHub.Data.Models;
using GymHub.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace GymHub.Web.Services
{
    public class GenderService : IGenderService
    {
        private readonly ApplicationDbContext context;
        public GenderService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(string name)
        {
            this.context.Add(new Gender() { Name = name });
            this.context.SaveChanges();
        }

        public bool GenderExists(string name)
        {
            return this.context.Genders.Any(x => x.Name == name);
        }

        public List<Gender> GetAllGenders()
        {
            return this.context.Genders.ToList();
        }

        public string GetGenderIdByName(string name)
        {
            return this.context.Genders.FirstOrDefault(x => x.Name == name).Id;
        }
    }
}

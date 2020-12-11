using GymHub.Data.Data;
using GymHub.Data.Models;
using GymHub.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.CountryService
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext context;

        public CountryService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public bool CountryExists(string countryCode, bool hardCheck = false)
        {
            return this.context.Countries.IgnoreAllQueryFilters(hardCheck).Any(x => x.Code == countryCode);
        }

        public async Task AddAsync(string name, string code)
        {
            await this.context.AddAsync(new Country { Name = name, Code = code });
            await this.context.SaveChangesAsync();
        }

        public List<Country> GetAllCountries()
        {
            return this.context.Countries.ToList();
        }

        public Country GetCountryByCode(string countryCode)
        {
            return this.context.Countries.First(x => x.Code == countryCode);
        }
    }
}

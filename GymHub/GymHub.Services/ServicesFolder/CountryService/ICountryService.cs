using GymHub.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymHub.Services.ServicesFolder.CountryService
{
    public interface ICountryService
    {
        public List<Country> GetAllCountries();
        public bool CountryExists(string countryCode, bool hardCheck = false);
        public Task AddAsync(string name, string code);
        public Country GetCountryByCode(string countryCode);
    }
}

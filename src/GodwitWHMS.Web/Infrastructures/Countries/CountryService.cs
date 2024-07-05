using GodwitWHMS.Infrastructures.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GodwitWHMS.Infrastructures.Countries
{
    public class CountryService : ICountryService
    {

        private readonly ApplicationDbContext _context;

        public CountryService()
        {
        }

        public CountryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                RegionInfo region = new RegionInfo(ci.Name);
                string countryName = region.DisplayName;

                if (!countries.Any(c => c.Text == countryName))
                {
                    var countryItem = new SelectListItem
                    {
                        Value = countryName,
                        Text = countryName
                    };

                    countries.Add(countryItem);
                }
            }

            return countries;
        }


        public async Task<ICollection<SelectListItem>> GetCountriesAsync()
        {
            return await _context.Country
                .Select(c => new SelectListItem
                {
                    Value = c.CountryCode,
                    Text = c.CountryName
                })
                .ToListAsync();
        }
    }
}

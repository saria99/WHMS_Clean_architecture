using GodwitWHMS.Applications.Companies;
using GodwitWHMS.Infrastructures.Countries;
using GodwitWHMS.Infrastructures.Currencies;
using GodwitWHMS.Infrastructures.TimeZones;
using GodwitWHMS.Models.Entities;

namespace GodwitWHMS.Data.System
{
    public static class DefaultCompany
    {
        public static async Task GenerateAsync(
            CompanyService? companyService,
            ICurrencyService? currencyService,
            ITimeZoneService? timezoneService,
            ICountryService? countryService,
            ApplicationUser? creator
            )
        {
            if (companyService != null)
            {

                var defaultCompany = new Company
                {
                    Name = "Default Company, LLC.",
                    Currency = currencyService?.GetCurrencies().FirstOrDefault(x => x.Value.Equals("US$"))?.Value ?? "US$",
                    TimeZone = timezoneService?.GetAllTimeZones().FirstOrDefault(x => x.Value.Equals("SE Asia Standard Time"))?.Value ?? "SE Asia Standard Time",
                    Street = "123 Elm Street",
                    City = "Springfield",
                    State = "Illinois (IL)",
                    ZipCode = "62701",
                    Country = countryService?.GetCountries().FirstOrDefault(x => x.Value.Equals("United States"))?.Value ?? "United States",
                    CreatedByUserId = creator?.Id

                };

                await companyService.AddAsync(defaultCompany);

            }
        }
    }
}

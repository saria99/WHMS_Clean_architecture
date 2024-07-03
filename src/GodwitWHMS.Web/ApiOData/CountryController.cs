using GodwitWHMS.Applications.Features.Countries;
using GodwitWHMS.Domain.DTOs;
using GodwitWHMS.Infrastructures.Countries;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GodwitWHMS.ApiOData
{
    public class CountryController : ODataController
    {
        private readonly CountryServicev2 _countryService;

        public CountryController(CountryServicev2 countryService)
        {
            _countryService = countryService;
        }

        [EnableQuery]
        public IQueryable<CountryDto> Get()
        {
            return _countryService
                .GetAll()
                .Select(c => new CountryDto
                {
                    Id = c.Id,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName,
                    CreatedAtUtc = c.CreatedAtUtc,
                    RowGuid = c.RowGuid,
                });
        }
    }
}

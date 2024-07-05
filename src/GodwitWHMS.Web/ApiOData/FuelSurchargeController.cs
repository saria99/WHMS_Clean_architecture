using GodwitWHMS.Applications.Features.FuelSurcharges;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{
    public class FuelSurchargeController : ODataController
    {
        private readonly FuelSurchargeService _fuelSurchargeService;

        public FuelSurchargeController(FuelSurchargeService fuelSurchargeService)
        {
            _fuelSurchargeService = fuelSurchargeService;
        }

        [EnableQuery]
        public IQueryable<FuelSurchargeDto> Get()
        {
            return _fuelSurchargeService
                .GetAll()
                .Include(fs => fs.Carrier)
                .Include(fs => fs.OriginCountry)
                .Include(fs => fs.DestinationCountry)
                .Select(fs => new FuelSurchargeDto
                {
                    Id = fs.Id,
                    CarrierId = fs.CarrierId,
                    CarrierName = fs.Carrier.CarrierName,
                    OriginCountryId = fs.OriginCountryId,
                    OriginCountryName = fs.OriginCountry.CountryName,
                    DestinationCountryId = fs.DestinationCountryId,
                    DestinationCountryName = fs.DestinationCountry.CountryName,
                    EffectiveDate = fs.EffectiveDate,
                    FuelSurchargePercentage = fs.FuelSurchargePercentage,
                    RowGuid = fs.RowGuid,
                    CreatedAtUtc = fs.CreatedAtUtc,
                });
        }
    }
}

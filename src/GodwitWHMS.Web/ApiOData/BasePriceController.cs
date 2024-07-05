using GodwitWHMS.Applications.Features.BasePrices;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{
    public class BasePriceController : ODataController
    {
        private readonly BasePriceService _basePriceService;

        public BasePriceController(BasePriceService basePriceService)
        {
            _basePriceService = basePriceService;
        }

        [EnableQuery]
        public IQueryable<BasePriceDto> Get()
        {
            return _basePriceService
                .GetAll()
                .Include(bp => bp.Carrier)
                .Include(bp => bp.OriginCountry)
                .Include(bp => bp.DestinationCountry)
                .Select(bp => new BasePriceDto
                {
                    Id = bp.Id,
                    CarrierId = bp.CarrierId,
                    CarrierName = bp.Carrier.CarrierName,
                    OriginCountryId = bp.OriginCountryId,
                    OriginCountryName = bp.OriginCountry.CountryName,
                    DestinationCountryId = bp.DestinationCountryId,
                    DestinationCountryName = bp.DestinationCountry.CountryName,
                    Weight = bp.Weight,
                    Price = bp.Price,
                    RowGuid = bp.RowGuid,
                    CreatedAtUtc = bp.CreatedAtUtc,
                });
        }
    }
}

using GodwitWHMS.Applications.Features.BasePrices;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace GodwitWHMS.ApiOData
{
    public class BasePriceController : ODataController
    {
        private readonly BasePriceService _basePriceService;
        private readonly CalculatedPriceService _calculatedPriceService;

        public BasePriceController(BasePriceService basePriceService, CalculatedPriceService calculatedPriceService)
        {
            _basePriceService = basePriceService;
            _calculatedPriceService = calculatedPriceService;
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
                    PriceWithFuelSurcharge = bp.PriceWithFuelSurcharge,
                    TotalPrice = bp.TotalPrice,
                    RowGuid = bp.RowGuid,
                    CreatedAtUtc = bp.CreatedAtUtc,
                });
        }




        [HttpGet("Cheapest")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCheapestPrices()
        {
            var cheapestPrices = await _calculatedPriceService.GetAllCheapestPricesAsync();
            return Ok(cheapestPrices.Select(cp => new CalculatedPriceDto
            {
                Id = cp.Id,
                BasePriceId = cp.BasePriceId,
                ServiceType = cp.ServiceType,
                PriceWithFuelSurcharge = cp.PriceWithFuelSurcharge,
                TotalPrice = cp.TotalPrice,
                IsCheapest = cp.IsCheapest,
                RowGuid = cp.RowGuid,
                CreatedAtUtc = cp.CreatedAtUtc,
            }));
        }
    }


}

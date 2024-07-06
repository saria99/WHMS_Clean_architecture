using GodwitWHMS.Applications.Features.BasePrices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GodwitWHMS.ApiOData
{
    public class CalculatedPriceController : ODataController
    {
        private readonly CalculatedPriceService _calculatedPriceService;

        public CalculatedPriceController(CalculatedPriceService calculatedPriceService)
        {
            _calculatedPriceService = calculatedPriceService;
        }

        [EnableQuery]
        public IQueryable<CalculatedPriceDto> Get()
        {
            return _calculatedPriceService
                .GetAllCheapestPrices()
                .Include(cp => cp.BasePrice)
                    .ThenInclude(bp => bp.Carrier)
                .Include(cp => cp.BasePrice)
                    .ThenInclude(bp => bp.OriginCountry)
                .Include(cp => cp.BasePrice)
                    .ThenInclude(bp => bp.DestinationCountry)
                .Select(cp => new CalculatedPriceDto
                {
                    Id = cp.Id,
                    BasePriceId = cp.BasePriceId,
                    CarrierName = cp.BasePrice.Carrier.CarrierName,
                    OriginCountryName = cp.BasePrice.OriginCountry.CountryName,
                    DestinationCountryName = cp.BasePrice.DestinationCountry.CountryName,
                    ServiceType = cp.ServiceType.ToString(),
                    PriceWithFuelSurcharge = cp.PriceWithFuelSurcharge,
                    Weight = cp.Weight,
                    TotalPrice = cp.TotalPrice,
                    IsCheapest = cp.IsCheapest,
                    RowGuid = cp.RowGuid,
                });
        }
        //public async Task<IActionResult> Get()
        //{
        //    var cheapestPrices = await _calculatedPriceService.GetAllCheapestPricesAsync();
        //    return Ok(cheapestPrices.Select(cp => new CalculatedPriceDto
        //    {
        //        Id = cp.Id,
        //        BasePriceId = cp.BasePriceId,
        //        ServiceType = cp.ServiceType,
        //        PriceWithFuelSurcharge = cp.PriceWithFuelSurcharge,
        //        TotalPrice = cp.TotalPrice,
        //        IsCheapest = cp.IsCheapest,
        //        RowGuid = cp.RowGuid,
        //        CreatedAtUtc = cp.CreatedAtUtc,
        //    }));
        //}
    }


}

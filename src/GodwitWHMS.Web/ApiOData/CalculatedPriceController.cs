using GodwitWHMS.Applications.Features.BasePrices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
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
        public async Task<IActionResult> Get()
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

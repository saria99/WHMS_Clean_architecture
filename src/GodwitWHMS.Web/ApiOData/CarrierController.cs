using GodwitWHMS.Applications.Features.Carriers;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{
    public class CarrierController : ODataController
    {
        private readonly CarrierService _carrierService;

        public CarrierController(CarrierService carrierService)
        {
            _carrierService = carrierService;
        }

        [EnableQuery]
        public IQueryable<CarrierDto> Get()
        {
            return _carrierService
                .GetAll()
                .Select(c => new CarrierDto
                {
                    Id = c.Id,
                    RowGuid = c.RowGuid,
                    CarrierName = c.CarrierName,
                    CreatedAtUtc = c.CreatedAtUtc,
                });
        }
    }
}

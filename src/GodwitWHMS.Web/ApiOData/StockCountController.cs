using GodwitWHMS.Applications.StockCounts;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{
    public class StockCountController : ODataController
    {
        private readonly StockCountService _stockCountService;

        public StockCountController(StockCountService stockCountService)
        {
            _stockCountService = stockCountService;
        }

        [EnableQuery]
        public IQueryable<StockCountDto> Get()
        {
            return _stockCountService
                .GetAll()
                .Include(x => x.Warehouse)
                .Select(rec => new StockCountDto
                {
                    Id = rec.Id,
                    Number = rec.Number,
                    CountDate = rec.CountDate,
                    Status = rec.Status,
                    Warehouse = rec.Warehouse!.Name,
                    RowGuid = rec.RowGuid,
                    CreatedAtUtc = rec.CreatedAtUtc,
                });
        }




    }
}

using GodwitWHMS.Applications.Features.Commissions;
using GodwitWHMS.Domain.DTOs;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace GodwitWHMS.ApiOData
{
    public class CommissionController : ODataController
    {
        private readonly CommissionService _commissionService;

        public CommissionController(CommissionService commissionService)
        {
            _commissionService = commissionService;
        }

        [EnableQuery]
        public IQueryable<CommissionDto> Get()
        {
            return _commissionService
                .GetAll()
                .Select(c => new CommissionDto
                {
                    Id = c.Id,
                    ServiceType = c.ServiceType,
                    CommissionPercentage = c.CommissionPercentage,
                    EffectiveDate = c.EffectiveDate,
                    RowGuid = c.RowGuid,
                    CreatedAtUtc = c.CreatedAtUtc,
                })
                .AsQueryable();
        }
    }
}

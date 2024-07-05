using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.Features.Commissions
{
    public class CommissionService : Repository<Commission>
    {
        public CommissionService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }

        public decimal GetCurrentCommission(string serviceType)
        {
            var currentCommission = _context.Set<Commission>()
                .Where(c => c.ServiceType == serviceType)
                .OrderByDescending(c => c.EffectiveDate)
                .FirstOrDefault();

            return currentCommission != null ? currentCommission.CommissionPercentage : 0;
        }


    }
}

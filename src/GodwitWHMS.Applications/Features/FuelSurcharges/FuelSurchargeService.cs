using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.Features.FuelSurcharges
{
    public class FuelSurchargeService : Repository<FuelSurcharge>
    {
        public FuelSurchargeService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
            base(context, httpContextAccessor, auditColumnTransformer)
        {
        }
    }
}

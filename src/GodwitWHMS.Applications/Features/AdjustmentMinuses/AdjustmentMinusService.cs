
using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.AdjustmentMinuss
{
    public class AdjustmentMinusService : Repository<AdjustmentMinus>
    {
        public AdjustmentMinusService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }


    }
}

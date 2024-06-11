using GodwitWHMS.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;

namespace GodwitWHMS.Applications.PurchaseReturns
{
    public class PurchaseReturnService : Repository<PurchaseReturn>
    {
        public PurchaseReturnService(
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

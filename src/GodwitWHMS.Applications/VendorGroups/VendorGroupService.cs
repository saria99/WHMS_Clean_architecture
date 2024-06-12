using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.VendorGroups
{
    public class VendorGroupService : Repository<VendorGroup>
    {
        public VendorGroupService(
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

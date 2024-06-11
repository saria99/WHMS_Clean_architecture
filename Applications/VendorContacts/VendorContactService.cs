using GodwitWHMS.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;

namespace GodwitWHMS.Applications.VendorContacts
{
    public class VendorContactService : Repository<VendorContact>
    {
        public VendorContactService(
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

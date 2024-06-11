using GodwitWHMS.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;

namespace GodwitWHMS.Applications.Scrappings
{
    public class ScrappingService : Repository<Scrapping>
    {
        public ScrappingService(
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

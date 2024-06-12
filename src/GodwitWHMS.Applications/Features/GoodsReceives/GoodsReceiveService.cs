using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace GodwitWHMS.Applications.GoodsReceives
{
    public class GoodsReceiveService : Repository<GoodsReceive>
    {
        public GoodsReceiveService(
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

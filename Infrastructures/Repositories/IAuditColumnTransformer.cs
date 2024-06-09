using GodwitWHMS.Data;
using GodwitWHMS.Models.Contracts;

namespace GodwitWHMS.Infrastructures.Repositories
{
    public interface IAuditColumnTransformer
    {
        Task TransformAsync(IHasAudit entity, ApplicationDbContext context);
    }
}

using GodwitWHMS.Domain.Models.Contracts;
using GodwitWHMS.Infrastructures.Data;

namespace GodwitWHMS.Infrastructures.Repositories
{
    public interface IAuditColumnTransformer
    {
        Task TransformAsync(IHasAudit entity, ApplicationDbContext context);
    }
}

using GodwitWHMS.Data;
using GodwitWHMS.Domain.Models.Contracts;

namespace GodwitWHMS.Infrastructures.Repositories
{
    public interface IAuditColumnTransformer
    {
        Task TransformAsync(IHasAudit entity, ApplicationDbContext context);
    }
}

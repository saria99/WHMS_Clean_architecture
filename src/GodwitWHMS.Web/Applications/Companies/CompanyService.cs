using GodwitWHMS.Data;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.Applications.Companies
{
    public class CompanyService : Repository<Company>
    {
        public CompanyService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer) :
                base(
                    context,
                    httpContextAccessor,
                    auditColumnTransformer)
        {
        }

        public async Task<Company?> GetDefaultCompanyAsync()
        {
            return await _context.Company.FirstOrDefaultAsync();
        }

    }
}

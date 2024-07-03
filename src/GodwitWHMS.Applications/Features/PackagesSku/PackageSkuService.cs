using GodwitWHMS.Domain.Models.Entities;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace GodwitWHMS.Applications.Features.PackagesSku
{
    public class PackageSkuService : Repository<PackageSku>
    {
        private readonly IDistributedCache _cache;
        private static readonly object lockObject = new object();

        public PackageSkuService(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IAuditColumnTransformer auditColumnTransformer,
            IDistributedCache cache) :
                base(context, httpContextAccessor, auditColumnTransformer)
        {
            _cache = cache;
        }


        public async Task<PackageSku> GenerateSkuAsync(string scannedCode)
        {
            var newSkuCode = GenerateNumber(nameof(PackageSku), "GW-", "", false, 8);

            var newSku = new PackageSku { Code = newSkuCode, ScannedCode = scannedCode };
            await AddAsync(newSku);

            return newSku;
        }

        public async Task<PackageSku> GetBySkuAsync(string skuCode)
        {
            return await _context.Set<PackageSku>().FirstOrDefaultAsync(s => s.Code == skuCode);
        }

        public async Task<PackageSku> GetByIdAsync(int id)
        {
            return await _context.Set<PackageSku>().FindAsync(id);
        }
        public string GenerateNumber(string entityName, string prefix, string suffix, bool useDate = true, int padding = 4)
        {
            if (string.IsNullOrEmpty(entityName))
            {
                throw new ArgumentException("Parameter entityName must not be null");
            }

            lock (lockObject)
            {
                int newCount = _context.Set<PackageSku>().Count() + 1;
                string formattedNumber = $"{prefix}{newCount.ToString().PadLeft(padding, '0')}{(useDate ? DateTime.Now.ToString("yyyyMMdd") : "")}{suffix}";
                return formattedNumber;
            }
        }

    }
}

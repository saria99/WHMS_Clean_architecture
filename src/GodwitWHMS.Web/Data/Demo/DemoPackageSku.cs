using GodwitWHMS.Applications.Features.PackagesSku;
using GodwitWHMS.Domain.Models.Entities;

namespace GodwitWHMS.Data.Demo
{
    public static class DemoPackageSku
    {
        public static async Task GenerateAsync(IServiceProvider services)
        {
            var packageSkuService = services.GetRequiredService<PackageSkuService>();

            await packageSkuService.AddAsync(new PackageSku
            {
                Code = "GW-00000001",
                ScannedCode = "SCANNED-CODE-001"
            });

            await packageSkuService.AddAsync(new PackageSku
            {
                Code = "GW-00000002",
                ScannedCode = "SCANNED-CODE-002"
            });
        }
    }
}

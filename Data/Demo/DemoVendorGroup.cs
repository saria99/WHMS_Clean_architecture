using GodwitWHMS.Applications.VendorGroups;
using GodwitWHMS.Models.Entities;

namespace GodwitWHMS.Data.Demo
{
    public static class DemoVendorGroup
    {
        public static async Task GenerateAsync(IServiceProvider services)
        {
            var service = services.GetRequiredService<VendorGroupService>();

            await service.AddAsync(new VendorGroup { Name = "Manufacture" });
            await service.AddAsync(new VendorGroup { Name = "Supplier" });
            await service.AddAsync(new VendorGroup { Name = "Service Provider" });
            await service.AddAsync(new VendorGroup { Name = "Distributor" });
            await service.AddAsync(new VendorGroup { Name = "Freelancer" });
        }
    }
}

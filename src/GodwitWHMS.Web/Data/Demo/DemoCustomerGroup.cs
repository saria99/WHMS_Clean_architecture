﻿using GodwitWHMS.Applications.CustomerGroups;
using GodwitWHMS.Domain.Models.Entities;

namespace GodwitWHMS.Data.Demo
{
    public static class DemoCustomerGroup
    {
        public static async Task GenerateAsync(IServiceProvider services)
        {
            var service = services.GetRequiredService<CustomerGroupService>();

            await service.AddAsync(new CustomerGroup { Name = "Corporate" });
            await service.AddAsync(new CustomerGroup { Name = "Government" });
            await service.AddAsync(new CustomerGroup { Name = "Foundation" });
            await service.AddAsync(new CustomerGroup { Name = "Military" });
            await service.AddAsync(new CustomerGroup { Name = "Education" });
            await service.AddAsync(new CustomerGroup { Name = "Hospitality" });
        }
    }
}

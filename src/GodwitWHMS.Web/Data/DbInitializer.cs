﻿using GodwitWHMS.Applications.ApplicationUsers;
using GodwitWHMS.Applications.Companies;
using GodwitWHMS.Applications.AppSettings;
using GodwitWHMS.Data.Demo;
using GodwitWHMS.Data.System;
using GodwitWHMS.Infrastructures.Countries;
using GodwitWHMS.Infrastructures.Currencies;
using GodwitWHMS.Infrastructures.Images;
using GodwitWHMS.Infrastructures.Menus;
using GodwitWHMS.Infrastructures.TimeZones;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using GodwitWHMS.Infrastructures.Data;

namespace GodwitWHMS.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider services
            )
        {

            var context = services.GetRequiredService<ApplicationDbContext>();
            if (context.Users.Any())
            {
                return;
            }


            var appConfig = services.GetRequiredService<IOptions<ApplicationConfiguration>>();
            var fileImageService = services.GetRequiredService<IFileImageService>();
            var countryService = services.GetRequiredService<ICountryService>();
            var currencyService = services.GetRequiredService<ICurrencyService>();
            var timeZoneService = services.GetRequiredService<ITimeZoneService>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var companyService = services.GetRequiredService<CompanyService>();
            var applicationUserService = services.GetRequiredService<ApplicationUserService>();
            var menuService = services.GetRequiredService<MenuService>();

            var creator = await applicationUserService.GetAll()
                .Where(x => x.UserName == appConfig.Value.DefaultAdminEmail)
                .FirstOrDefaultAsync();

            await DefaultCompany.GenerateAsync(companyService, currencyService, timeZoneService, countryService, creator);
            await DefaultRole.GenerateAsync(roleManager, menuService);
            await DefaultUser.GenerateAsync(userManager, appConfig, fileImageService, companyService, menuService);

            await DefaultSystemWarehouse.GenerateAsync(services);

            if (appConfig.Value.IsDemoVersion)
            {
                await DemoUser.GenerateAsync(userManager, appConfig, fileImageService, companyService, menuService);
                await DemoCustomerGroup.GenerateAsync(services);
                await DemoCustomerCategory.GenerateAsync(services);
                await DemoVendorGroup.GenerateAsync(services);
                await DemoVendorCategory.GenerateAsync(services);
                await DemoWarehouse.GenerateAsync(services);
                await DemoCustomer.GenerateAsync(services);
                await DemoVendor.GenerateAsync(services);
                await DemoUnitMeasure.GenerateAsync(services);
                await DemoProductGroup.GenerateAsync(services);
                await DemoProduct.GenerateAsync(services);
                await DemoCustomerContact.GenerateAsync(services);
                await DemoVendorContact.GenerateAsync(services);
                await DemoTax.GenerateAsync(services);
                await DemoSalesOrder.GenerateAsync(services);
                await DemoPurchaseOrder.GenerateAsync(services);
                await DemoDeliveryOrder.GenerateAsync(services);
                await DemoGoodsReceive.GenerateAsync(services);
                await DemoSalesReturn.GenerateAsync(services);
                await DemoPurchaseReturn.GenerateAsync(services);
                await DemoTransferOut.GenerateAsync(services);
                await DemoTransferIn.GenerateAsync(services);
                await DemoAdjustmentMinus.GenerateAsync(services);
                await DemoAdjustmentPlus.GenerateAsync(services);
                await DemoScrapping.GenerateAsync(services);
                await DemoStockCount.GenerateAsync(services);
            }
        }

        public static DateTime[] GetRandomDays(int year, int month, int count)
        {
            Random random = new Random();
            int daysInMonth = DateTime.DaysInMonth(year, month);
            DateTime[] dates = new DateTime[Math.Min(count, daysInMonth)];

            for (int i = 0; i < dates.Length; i++)
            {
                dates[i] = DateTime.MinValue;
            }

            for (int i = 0; i < count; i++)
            {
                int day = random.Next(1, daysInMonth + 1);
                DateTime date = new DateTime(year, month, day);

                while (Array.IndexOf(dates, date) != -1)
                {
                    day = random.Next(1, daysInMonth + 1);
                    date = new DateTime(year, month, day);
                }

                dates[i] = date;
            }

            return dates;
        }

        public static string GetRandomString(string[] strings, Random random)
        {
            int randomIndex = random.Next(0, strings.Length);
            return strings[randomIndex];
        }
        public static double GetRandomValue(double[] targetValues, Random random)
        {
            int randomIndex = random.Next(0, targetValues.Length);
            return targetValues[randomIndex];

        }
        public static int GetRandomValue(int[] targetValues, Random random)
        {
            int randomIndex = random.Next(0, targetValues.Length);
            return targetValues[randomIndex];

        }
    }
}

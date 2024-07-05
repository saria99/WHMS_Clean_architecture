using GodwitWHMS.Applications.AdjustmentMinuss;
using GodwitWHMS.Applications.AdjustmentPluss;
using GodwitWHMS.Applications.ApplicationUsers;
using GodwitWHMS.Applications.Companies;
using GodwitWHMS.Applications.CustomerCategories;
using GodwitWHMS.Applications.CustomerContacts;
using GodwitWHMS.Applications.CustomerGroups;
using GodwitWHMS.Applications.Customers;
using GodwitWHMS.Applications.DeliveryOrders;
using GodwitWHMS.Applications.GoodsReceives;
using GodwitWHMS.Applications.InventoryTransactions;
using GodwitWHMS.Applications.LogAnalytics;
using GodwitWHMS.Applications.LogErrors;
using GodwitWHMS.Applications.LogSessions;
using GodwitWHMS.Applications.NumberSequences;
using GodwitWHMS.Applications.ProductGroups;
using GodwitWHMS.Applications.Products;
using GodwitWHMS.Applications.PurchaseOrderItems;
using GodwitWHMS.Applications.PurchaseOrders;
using GodwitWHMS.Applications.PurchaseReturns;
using GodwitWHMS.Applications.SalesOrderItems;
using GodwitWHMS.Applications.SalesOrders;
using GodwitWHMS.Applications.SalesReturns;
using GodwitWHMS.Applications.Scrappings;
using GodwitWHMS.Applications.StockCounts;
using GodwitWHMS.Applications.Taxes;
using GodwitWHMS.Applications.TransferIns;
using GodwitWHMS.Applications.TransferOuts;
using GodwitWHMS.Applications.UnitMeasures;
using GodwitWHMS.Applications.VendorCategories;
using GodwitWHMS.Applications.VendorContacts;
using GodwitWHMS.Applications.VendorGroups;
using GodwitWHMS.Applications.Vendors;
using GodwitWHMS.Applications.Warehouses;
using GodwitWHMS.Infrastructures.BarCode;
using GodwitWHMS.Infrastructures.Countries;
using GodwitWHMS.Infrastructures.Currencies;
using GodwitWHMS.Infrastructures.Docs;
using GodwitWHMS.Infrastructures.Emails;
using GodwitWHMS.Infrastructures.Images;
using GodwitWHMS.Infrastructures.Menus;
using GodwitWHMS.Infrastructures.Repositories;
using GodwitWHMS.Infrastructures.TimeZones;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using GodwitWHMS.Applications.Features.PackagesSku;
using GodwitWHMS.Applications.Features.Countries;
using GodwitWHMS.Applications.Features.Carriers;
using GodwitWHMS.Applications.Features.BasePrices;
using GodwitWHMS.Applications.Features.FuelSurcharges;
using GodwitWHMS.Applications.Features.Commissions;

namespace GodwitWHMS
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAllCustomServices(this IServiceCollection services)
        {
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IEmailSender, SMTPEmailService>();
            services.AddTransient<IBarcodeGenerator, BarcodeGenerator>();
            services.AddScoped<IFileImageService, FileImageService>();
            services.AddScoped<IFileDocumentService, FileDocumentService>();
            services.AddScoped<ITimeZoneService, TimeZoneService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IAuditColumnTransformer, AuditColumnTransformer>();
            services.AddScoped<MenuService>();
            services.AddScoped<CompanyService>();
            services.AddScoped<ApplicationUserService>();
            services.AddScoped<NumberSequenceService>();
            services.AddScoped<LogErrorService>();
            services.AddScoped<LogSessionService>();
            services.AddScoped<LogAnalyticService>();
            services.AddScoped<CustomerGroupService>();
            services.AddScoped<CustomerCategoryService>();
            services.AddScoped<VendorGroupService>();
            services.AddScoped<VendorCategoryService>();
            services.AddScoped<WarehouseService>();
            services.AddScoped<CustomerService>();
            services.AddScoped<VendorService>();
            services.AddScoped<UnitMeasureService>();
            services.AddScoped<ProductGroupService>();
            services.AddScoped<ProductService>();
            services.AddScoped<CustomerContactService>();
            services.AddScoped<VendorContactService>();
            services.AddScoped<TaxService>();
            services.AddScoped<SalesOrderService>();
            services.AddScoped<SalesOrderItemService>();
            services.AddScoped<PurchaseOrderService>();
            services.AddScoped<PurchaseOrderItemService>();
            services.AddScoped<InventoryTransactionService>();
            services.AddScoped<DeliveryOrderService>();
            services.AddScoped<GoodsReceiveService>();
            services.AddScoped<SalesReturnService>();
            services.AddScoped<PurchaseReturnService>();
            services.AddScoped<TransferInService>();
            services.AddScoped<TransferOutService>();
            services.AddScoped<StockCountService>();
            services.AddScoped<AdjustmentMinusService>();
            services.AddScoped<AdjustmentPlusService>();
            services.AddScoped<ScrappingService>();

            services.AddScoped<PackageSkuService>();
            services.AddScoped<CountryServicev2>();
            services.AddScoped<CarrierService>();
            services.AddScoped<BasePriceService>();
            services.AddScoped<FuelSurchargeService>();
            services.AddScoped<CommissionService>();
            services.AddScoped<CalculatedPriceService>();

            return services;
        }
    }
}

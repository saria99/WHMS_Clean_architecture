using Indotalent.Applications.Companies;
using Indotalent.Applications.GoodsReceives;
using Indotalent.Applications.InventoryTransactions;
using Indotalent.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Indotalent.Pages.GoodsReceives
{
    [AllowAnonymous]
    public class GoodsReceivePdfModel : PageModel
    {
        private readonly GoodsReceiveService _goodsReceiveService;
        private readonly InventoryTransactionService _inventoryTransactionService;
        private readonly CompanyService _companyService;
        public GoodsReceivePdfModel(
            GoodsReceiveService goodsReceiveService,
            InventoryTransactionService inventoryTransactionService,
            CompanyService companyService)
        {
            _goodsReceiveService = goodsReceiveService;
            _inventoryTransactionService = inventoryTransactionService;
            _companyService = companyService;
        }

        public GoodsReceive? GoodsReceive { get; set; }
        public List<InventoryTransaction>? InventoryTransactions { get; set; }
        public Company? Company { get; set; }
        public Vendor? Vendor { get; set; }
        public string? CompanyAddress { get; set; }
        public string? VendorAddress { get; set; }

        public async Task OnGetAsync(int? id)
        {
            Company = await _companyService.GetDefaultCompanyAsync();

            CompanyAddress = string.Join(", ", new List<string>()
            {
                Company?.Street ?? string.Empty,
                Company?.City ?? string.Empty,
                Company?.State ?? string.Empty,
                Company?.Country ?? string.Empty,
                Company?.ZipCode ?? string.Empty
            }.Where(s => !string.IsNullOrEmpty(s)));

            GoodsReceive = await _goodsReceiveService
                .GetAll()
                .Include(x => x.PurchaseOrder)
                    .ThenInclude(x => x!.Vendor)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            InventoryTransactions = await _inventoryTransactionService
                .GetAll()
                .Where(x => x.ModuleId == id && x.ModuleName == nameof(GoodsReceive))
                .Include(x => x.Product)
                    .ThenInclude(x => x!.UnitMeasure)
                .ToListAsync();

            Vendor = GoodsReceive!.PurchaseOrder!.Vendor;

            VendorAddress = string.Join(", ", new List<string>()
            {
                Vendor?.Street ?? string.Empty,
                Vendor?.City ?? string.Empty,
                Vendor?.State ?? string.Empty,
                Vendor?.Country ?? string.Empty,
                Vendor?.ZipCode ?? string.Empty

            }.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}

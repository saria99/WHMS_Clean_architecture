using GodwitWHMS.Applications.Companies;
using GodwitWHMS.Applications.InventoryTransactions;
using GodwitWHMS.Applications.Scrappings;
using GodwitWHMS.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.Pages.Scrappings
{
    [AllowAnonymous]
    public class ScrappingPdfModel : PageModel
    {
        private readonly ScrappingService _scrappingService;
        private readonly InventoryTransactionService _inventoryTransactionService;
        private readonly CompanyService _companyService;
        public ScrappingPdfModel(
            ScrappingService scrappingService,
            InventoryTransactionService inventoryTransactionService,
            CompanyService companyService)
        {
            _scrappingService = scrappingService;
            _inventoryTransactionService = inventoryTransactionService;
            _companyService = companyService;
        }

        public Scrapping? Scrapping { get; set; }
        public List<InventoryTransaction>? InventoryTransactions { get; set; }
        public Company? Company { get; set; }
        public Warehouse? Warehouse { get; set; }
        public string? CompanyAddress { get; set; }

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

            Scrapping = await _scrappingService
                .GetAll()
                .Include(x => x.Warehouse)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            Warehouse = Scrapping?.Warehouse;

            InventoryTransactions = await _inventoryTransactionService
                .GetAll()
                .Where(x => x.ModuleId == id && x.ModuleName == nameof(Scrapping))
                .Include(x => x.Product)
                    .ThenInclude(x => x!.UnitMeasure)
                .ToListAsync();
        }
    }
}

using AutoMapper;
using GodwitWHMS.Applications.Features.BasePrices;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace GodwitWHMS.Pages.BasePrices
{
    [Authorize]
    public class TotalPriceFormModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly CalculatedPriceService _basePriceService;

        public TotalPriceFormModel(
            IMapper mapper,
            CalculatedPriceService basePriceService)
        {
            _mapper = mapper;
            _basePriceService = basePriceService;
        }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;
        public string? Action { get; set; } = string.Empty;

        public async Task<IActionResult> OnGet()
        {
            await _basePriceService.CalculateAndStorePricesAsync();
            StatusMessage = "Prices have been successfully calculated.";
            return RedirectToPage("./BasePriceList");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await _basePriceService.CalculateAndStorePricesAsync();
            StatusMessage = "Prices have been successfully calculated.";
            return RedirectToPage("./BasePriceList");
        }
    }
}

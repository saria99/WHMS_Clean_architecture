using GodwitWHMS.Infrastructures.Pdfs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.StockCounts
{
    [AllowAnonymous]
    public class StockCountDownloadModel : PageModel
    {
        private readonly IPdfService _pdfService;
        public StockCountDownloadModel(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }
        public IActionResult OnGet(string? id)
        {
            string fileName = $"StockCount-{Guid.NewGuid()}.pdf";
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string htmlUrl = $"{baseUrl}/StockCounts/StockCountPdf/{id}";
            byte[] pdfBytes = _pdfService.CreatePdfFromPage(htmlUrl, fileName);
            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}

using GodwitWHMS.Infrastructures.Pdfs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.GoodsReceives
{
    [AllowAnonymous]
    public class GoodsReceiveDownloadModel : PageModel
    {
        private readonly IPdfService _pdfService;
        public GoodsReceiveDownloadModel(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }
        public IActionResult OnGet(string? id)
        {
            string fileName = $"GoodsReceive-{Guid.NewGuid()}.pdf";
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string htmlUrl = $"{baseUrl}/GoodsReceives/GoodsReceivePdf/{id}";
            byte[] pdfBytes = _pdfService.CreatePdfFromPage(htmlUrl, fileName);
            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}

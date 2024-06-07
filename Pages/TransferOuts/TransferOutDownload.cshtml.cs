using Indotalent.Infrastructures.Pdfs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Indotalent.Pages.TransferOuts
{
    [AllowAnonymous]
    public class TransferOutDownloadModel : PageModel
    {
        private readonly IPdfService _pdfService;
        public TransferOutDownloadModel(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }
        public IActionResult OnGet(string? id)
        {
            string fileName = $"TransferOut-{Guid.NewGuid()}.pdf";
            string baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            string htmlUrl = $"{baseUrl}/TransferOuts/TransferOutPdf/{id}";
            byte[] pdfBytes = _pdfService.CreatePdfFromPage(htmlUrl, fileName);
            return File(pdfBytes, "application/pdf", fileName);
        }

    }
}

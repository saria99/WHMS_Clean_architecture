using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.PurchaseOrderItems
{
    [Authorize]
    public class PurchaseOrderItemListModel : PageModel
    {
        public PurchaseOrderItemListModel() { }

        [TempData]
        public string StatusMessage { get; set; } = string.Empty;

        public void OnGet()
        {
            this.SetupViewDataTitleFromUrl();
            this.SetupStatusMessage();
            StatusMessage = this.ReadStatusMessage();
        }





    }
}

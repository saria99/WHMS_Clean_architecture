using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.LogAnalytics
{
    [Authorize]
    public class LogAnalyticListModel : PageModel
    {
        public LogAnalyticListModel() { }

        public void OnGet()
        {
            this.SetupViewDataTitleFromUrl();
        }

    }
}

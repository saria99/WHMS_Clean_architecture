using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.LogSessions
{
    [Authorize]
    public class LogSessionListModel : PageModel
    {
        public LogSessionListModel() { }

        public void OnGet()
        {
            this.SetupViewDataTitleFromUrl();
        }

    }
}

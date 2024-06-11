using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.LogErrors
{
    [Authorize]
    public class LogErrorListModel : PageModel
    {
        public LogErrorListModel() { }

        public void OnGet()
        {
            this.SetupViewDataTitleFromUrl();
        }

    }
}

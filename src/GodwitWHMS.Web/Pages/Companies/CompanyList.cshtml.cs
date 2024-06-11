using GodwitWHMS.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GodwitWHMS.Pages.Companies
{
    [Authorize]
    public class CompanyListModel : PageModel
    {
        public CompanyListModel() { }

        public void OnGet()
        {
            this.SetupViewDataTitleFromUrl();

        }



    }
}

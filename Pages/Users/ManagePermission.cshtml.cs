using Indotalent.Applications.ApplicationUsers;
using Indotalent.AppSettings;
using Indotalent.Infrastructures.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace Indotalent.Pages.Users
{
    [Authorize]
    public class ManagePermissionModel : PageModel
    {

        private readonly ApplicationUserService _applicationUserService;
        private readonly ApplicationConfiguration _appConfig;
        public ManagePermissionModel(
                ApplicationUserService applicationUserService,
                IOptions<ApplicationConfiguration> appConfig
            )
        {
            _applicationUserService = applicationUserService;
            _appConfig = appConfig.Value;
        }

        public string Id { get; set; } = string.Empty;

        public bool RoleEditable { get; set; } = false;

        public async Task OnGetAsync(string? id)
        {
            this.SetupViewDataTitleFromUrl();

            Id = Guid.Empty.ToString();

            if (!(string.IsNullOrEmpty(id) || id.Equals(Guid.Empty.ToString())))
            {
                var existing = await _applicationUserService.GetByIdAsync(id);
                if (existing == null)
                {
                    throw new Exception($"Unable to load: {id}");
                }

                Id = existing.Id;

                if (_appConfig.IsDemoVersion == true && existing.FullName == "Administrator")
                {
                    RoleEditable = false;
                }
                else
                {
                    RoleEditable = true;
                }
            }
        }
    }
}

using Indotalent.AppSettings;
using Indotalent.Infrastructures.Menus;
using Indotalent.Pages.Shared.Dashmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Indotalent.Data.System
{
    public static class DefaultRole
    {
        public static async Task GenerateAsync(
            RoleManager<IdentityRole>? roleManager,
            MenuService? menuService
            )
        {
            if (roleManager != null)
            {
                var roles = menuService?.GetAdminRoles() ?? Enumerable.Empty<string>();
                foreach ( var role in roles ) 
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                };

            }
        }
    }
}

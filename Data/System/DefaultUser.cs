using Indotalent.Applications.Companies;
using Indotalent.AppSettings;
using Indotalent.Infrastructures.Images;
using Indotalent.Infrastructures.Menus;
using Indotalent.Models.Entities;
using Indotalent.Pages.Shared.Dashmin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Indotalent.Data.System
{
    public static class DefaultUser
    {
        public static async Task GenerateAsync(
            UserManager<ApplicationUser>? userManager,
            IOptions<ApplicationConfiguration>? appConfig,
            IFileImageService? fileImageService,
            CompanyService? companyService,
            MenuService? menuService
            )
        {
            if (companyService != null)
            {
                var defaultCompany = await companyService.GetDefaultCompanyAsync();

                if (defaultCompany != null)
                {

                    var adminUser = new ApplicationUser
                    {
                        UserName = appConfig?.Value.DefaultAdminEmail,
                        Email = appConfig?.Value.DefaultAdminEmail,
                        FullName = appConfig?.Value.DefaultAdminFullName,
                        EmailConfirmed = true,
                        IsDefaultAdmin = true,
                        SelectedCompanyId = defaultCompany.Id,
                    };

                    if (userManager != null)
                    {

                        var adminPassword = appConfig?.Value.DefaultPassword;
                        await userManager.CreateAsync(adminUser, adminPassword ?? string.Empty);


                        var roles = menuService?.GetAdminRoles() ?? Enumerable.Empty<string>();
                        foreach (var role in roles)
                        {
                            await userManager.AddToRoleAsync(adminUser, role);
                        };


                        var avatarPath = Path.Combine("wwwroot", "default-avatar.png");
                        using (var stream = File.OpenRead(avatarPath))
                        {
                            var file = new FormFile(stream, 0, stream.Length, Path.GetFileName(stream.Name), Path.GetFileName(stream.Name))
                            {
                                Headers = new HeaderDictionary(),
                                ContentType = "image/png"
                            };

                            if (fileImageService != null)
                            {
                                var avatarId = await fileImageService.UploadImageAsync(file);
                                adminUser.Avatar = avatarId.ToString();
                                await userManager.UpdateAsync(adminUser);

                            }
                        }

                    }

                }

            }
        }
    }
}

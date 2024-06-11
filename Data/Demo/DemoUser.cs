using GodwitWHMS.Applications.Companies;
using GodwitWHMS.AppSettings;
using GodwitWHMS.Infrastructures.Images;
using GodwitWHMS.Infrastructures.Menus;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace GodwitWHMS.Data.Demo
{
    public static class DemoUser
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
                    var index = 1;
                    for (int i = 0; i < 10; i++)
                    {

                        var staffUser = new ApplicationUser
                        {
                            UserName = $"staff_{index}@gmail.com",
                            Email = $"staff_{index}@gmail.com",
                            FullName = $"staff_{index}",
                            EmailConfirmed = true,
                            IsDefaultAdmin = false,
                            SelectedCompanyId = defaultCompany.Id,
                        };

                        if (userManager != null)
                        {

                            var defaultPassword = appConfig?.Value.DefaultPassword;
                            await userManager.CreateAsync(staffUser, defaultPassword ?? string.Empty);

                            var roles = menuService?.GetNonAdminRoles() ?? Enumerable.Empty<string>();
                            foreach (var role in roles)
                            {
                                await userManager.AddToRoleAsync(staffUser, role);
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
                                    staffUser.Avatar = avatarId.ToString();
                                    await userManager.UpdateAsync(staffUser);

                                }
                            }

                        }

                        index++;
                    }

                }

            }
        }
    }
}

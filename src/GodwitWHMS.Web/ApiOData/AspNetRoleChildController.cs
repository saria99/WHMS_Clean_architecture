using AutoMapper;
using GodwitWHMS.Applications.ApplicationUsers;
using GodwitWHMS.Applications.NumberSequences;
using GodwitWHMS.Domain.DTOs;
using GodwitWHMS.Infrastructures.Menus;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace GodwitWHMS.ApiOData
{

    public class AspNetRoleChildController : ODataController
    {

        private readonly ApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MenuService _menuService;

        public AspNetRoleChildController(
            ApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            MenuService menuService)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _roleManager = roleManager; 
            _menuService = menuService;
        }

        private async Task<IQueryable<AspNetRoleChildDto>> AllAspNetRoleChildAsync(string userId)
        {
            List<AspNetRoleChildDto> result = new List<AspNetRoleChildDto>();
            var user = await _applicationUserService.GetByIdAsync(userId);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var aspNetRoles = await _roleManager.Roles.ToListAsync();

                foreach (var role in aspNetRoles)
                {
                    result.Add(new AspNetRoleChildDto
                    {
                        Id = role.Name,
                        UserName = user.UserName,
                        Permission = role.Name ?? string.Empty,
                        Module = _menuService.GetParentNode(role.Name ?? string.Empty)?.name,
                        GrantAccess = roles.Contains(role.Name ?? string.Empty)
                    });
                }
            }
            return result.AsQueryable();
        }

        [EnableQuery]
        public async Task<IQueryable<AspNetRoleChildDto>> Get()
        {
            const string HeaderKeyName = "ParentId";
            Request.Headers.TryGetValue(HeaderKeyName, out var headerValue);
            var parentId = headerValue.ToString();

            return await AllAspNetRoleChildAsync(parentId);
        }


        [EnableQuery]
        [HttpGet("{key}")]
        public async Task<SingleResult<AspNetRoleChildDto>> Get([FromODataUri] string key)
        {
            const string HeaderKeyName = "ParentId";
            Request.Headers.TryGetValue(HeaderKeyName, out var headerValue);
            var parentId = headerValue.ToString();

            var data = await AllAspNetRoleChildAsync(parentId);

            return SingleResult.Create(data
                .Where(x => x.Permission == key));
        }



        [HttpPatch]
        public async Task<IActionResult> Patch([FromODataUri] string key, [FromBody] Delta<AspNetRoleChildDto> delta)
        {
            try
            {

                const string HeaderKeyName = "ParentId";
                Request.Headers.TryGetValue(HeaderKeyName, out var headerValue);
                var parentId = headerValue.ToString();

                var data = await AllAspNetRoleChildAsync(parentId);

                var aspNetRole = data.Where(x => x.Id == key).FirstOrDefault();
                if (aspNetRole == null)
                {
                    return NotFound();
                }
                delta.Patch(aspNetRole);

                if (aspNetRole.GrantAccess == false)
                {
                    var user = await _applicationUserService.GetByIdAsync(parentId);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(aspNetRole.Permission))
                        {
                            var isInRole = await _userManager.IsInRoleAsync(user, aspNetRole.Permission);
                            if (isInRole)
                            {
                                await _userManager.RemoveFromRoleAsync(user, aspNetRole.Permission);
                            }

                        }
                    }

                } 
                else
                {
                    var user = await _applicationUserService.GetByIdAsync(parentId);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(aspNetRole.Permission))
                        {
                            var isInRole = await _userManager.IsInRoleAsync(user, aspNetRole.Permission);
                            if (!isInRole)
                            {
                                await _userManager.AddToRoleAsync(user, aspNetRole.Permission);
                            }

                        }
                    }

                }

                return Ok(aspNetRole);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

    }
}

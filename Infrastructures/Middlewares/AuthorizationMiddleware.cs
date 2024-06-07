using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.IO;
using Indotalent.Infrastructures.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;

namespace Indotalent.Infrastructures.Middlewares
{
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        private bool IsAllowAnonymous(PageActionDescriptor pageActionDescriptor)
        {
            return pageActionDescriptor.EndpointMetadata.Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                var pageActionDescriptor = endpoint.Metadata.GetMetadata<PageActionDescriptor>();

                if (pageActionDescriptor != null && IsAllowAnonymous(pageActionDescriptor))
                {
                    await _next(context);
                    return;
                }
            }

            string path = context.Request.Path.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(path) && !path.Contains("odata"))
            {
                string pageFolderName = path.ToPageFolderNameFromPath();
                if (!context.User.IsInRole(pageFolderName))
                {
                    await context.ForbidAsync();
                    return;
                }

            }
            await _next(context);
        }
    }
}

using Godwit.Shared;
using Godwit.Shared.Interfaces;
using Godwit.Web.Components;
using Godwit.Web.Services;
using GodwitWHMS.Infrastructures.Data;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Godwit.Shared.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//Register needed elements for authentication:
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, AuthorizationMiddlewareResultHandler>();
// This is our custom provider
builder.Services.AddScoped<ICustomAuthenticationStateProvider, BlazorAuthenticationStateProvider>();
// Use our custom provider when the app needs an AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider>(s
    => (BlazorAuthenticationStateProvider)s.GetRequiredService<ICustomAuthenticationStateProvider>());

// Use our custom provider when the app needs an AuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider>(s
    => (BlazorAuthenticationStateProvider)s.GetRequiredService<ICustomAuthenticationStateProvider>());

// Add device specific services used by Razor Class Library (Godwit.Shared)
builder.Services.AddScoped<IFormFactor, FormFactor>();

// Identity services
builder.Services.AddIdentityApiEndpoints<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

app.MapIdentityApi<IdentityUser>();//replace with your user class

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Godwit.Shared._Imports).Assembly); 

app.Run();

public class AuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    //Seems to fix the issue with 404 when navigating directly to pages that require auth.
    //This will properly redirect to the login page if not authorized. 
    //https://stackoverflow.com/questions/77693596/unexpected-authorization-behaviour-in-a-blazor-web-app-with-net-8
    public Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
    {
        return next(context);
    }
}

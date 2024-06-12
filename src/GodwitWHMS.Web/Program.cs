using GodwitWHMS;
using GodwitWHMS.Applications.AppSettings;
using GodwitWHMS.Infrastructures.Data;
using GodwitWHMS.Infrastructures.Middlewares;
using GodwitWHMS.Infrastructures.ODatas;
using GodwitWHMS.Infrastructures.Pdfs;
using GodwitWHMS.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;
using GodwitWHMS.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .Configure<IdentitySettings>(builder.Configuration.GetSection(IdentitySettings.IdentitySettingsName));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        var identitySettings = builder.Configuration.GetSection(IdentitySettings.IdentitySettingsName).Get<IdentitySettings>();
        if (identitySettings != null)
        {
            options.SignIn.RequireConfirmedAccount = identitySettings.RequireConfirmedAccount;
            options.Password.RequireDigit = identitySettings.RequireDigit;
            options.Password.RequiredLength = identitySettings.RequiredLength;
            options.Password.RequireNonAlphanumeric = identitySettings.RequireNonAlphanumeric;
            options.Password.RequireUppercase = identitySettings.RequireUppercase;
            options.Password.RequireLowercase = identitySettings.RequireLowercase;
            options.Lockout.DefaultLockoutTimeSpan = identitySettings.DefaultLockoutTimeSpan;
            options.Lockout.MaxFailedAccessAttempts = identitySettings.MaxFailedAccessAttempts;
        }
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddRoles<IdentityRole>()
    .AddDefaultTokenProviders();

builder.Services
    .ConfigureApplicationCookie(options =>
    {
        var appConfig = builder.Configuration.GetSection("ApplicationConfiguration").Get<ApplicationConfiguration>();
        if (appConfig != null)
        {
            options.LoginPath = new PathString(appConfig.LoginPage);
            options.LogoutPath = new PathString(appConfig.LogoutPage);
            options.AccessDeniedPath = new PathString(appConfig.AccessDeniedPage);
        }
    });

builder.Services
    .Configure<SmtpConfiguration>(builder.Configuration.GetSection("SmtpConfiguration"));

builder.Services
    .Configure<RegistrationConfiguration>(builder.Configuration.GetSection("RegistrationConfiguration"));

builder.Services.
    Configure<ApplicationConfiguration>(builder.Configuration.GetSection("ApplicationConfiguration"));

builder.Services.AddRazorPages();

builder.Services
    .AddAllCustomServices();

builder.Services
    .AddAutoMapper(typeof(Program));

builder.Services
    .AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

builder.Services
    .AddSingleton<IPdfService, PdfService>();

builder.Services
    .AddCustomOData();

builder.Services
    .AddSession();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var appConfig = services.GetRequiredService<IOptions<ApplicationConfiguration>>();
    if (appConfig.Value.IsDevelopment)
    {
        context.Database.EnsureCreated();//<===*** Development Only !!! ***
    }
    await DbInitializer.InitializeAsync(services);
}

app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<LogAnalyticMiddleware>();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseMiddleware<AuthorizationMiddleware>();

app.MapRazorPages();

app.MapControllers();

app.Run();

using Microsoft.Extensions.Logging;
using Godwit.MAUI.Services;
using Godwit.Shared;
using Godwit.Shared.Interfaces;

namespace Godwit.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //Ignore the interactive render settings in the shared razor class library by calling this method. 
            InteractiveRenderSettings.ConfigureBlazorHybridRenderModes();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            // Add device specific services used by Razor Class Library (Godwit.Shared)
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            return builder.Build();
        }
    }
}

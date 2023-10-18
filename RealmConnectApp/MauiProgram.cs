using Microsoft.Extensions.Logging;
using RealmConnectApp.ViewModels;
using RealmConnectApp.Views;

namespace RealmConnectApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<Dashboard>();
            builder.Services.AddSingleton<DashboardVM>();
            builder.Services.AddSingleton<LoginPageVM>();
            builder.Services.AddSingleton<LoginPage>();     


#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
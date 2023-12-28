using EquityX.Pages;
using EquityX.Services;
using EquityX.ViewModels;
using Microsoft.Extensions.Logging;

namespace EquityX
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

            // Adding custom services and dependicies 
            builder.Services.AddTransient<IFundsService, TestFundsService>();
            builder.Services.AddTransient<IStockService, TestStockService>();

            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<HomePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
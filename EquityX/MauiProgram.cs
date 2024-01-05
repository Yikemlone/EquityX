﻿using EquityX.Context;
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

            // Adding database context
            builder.Services.AddDbContext<EquityXDbContext>();
            
            // Adding Servives
            builder.Services.AddSingleton<IFundsService, FundsService>();
            builder.Services.AddSingleton<IStockService, StockService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();

            // Adding ViewModels
            builder.Services.AddSingleton<HomeViewModel>();

            // Adding Pages
            builder.Services.AddSingleton<HomePage>(); 
            builder.Services.AddSingleton<LoadingPage>();

            // Adding HttpClient
            builder.Services.AddSingleton<HttpClient>();

#if DEBUG
            builder.Services.AddTransient<IStockService, TestStockService>();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
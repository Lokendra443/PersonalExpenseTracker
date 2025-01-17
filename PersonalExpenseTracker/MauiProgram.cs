using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PersonalExpenseTracker.Services;

namespace PersonalExpenseTracker
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            

            

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
           
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

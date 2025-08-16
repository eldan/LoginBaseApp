using LoginBaseApp.Service;
using LoginBaseApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace LoginBaseApp
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
                    fonts.AddFont("MaterialSymbolsOutlined.ttf","MaterialSymbols");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<Views.RegisterPage>();
            builder.Services.AddTransient<ViewModels.RegisterPageViewModel>();

            builder.Services.AddSingleton<Views.LoginPage>();
            builder.Services.AddTransient<ViewModels.LoginPageViewModel>();

            builder.Services.AddSingleton<Views.UserListPage>();
            builder.Services.AddTransient<ViewModels.UserListPageViewModel>();

            builder.Services.AddSingleton<Views.UserDetailPage>();
            builder.Services.AddTransient<ViewModels.UserDetailPageViewModel>();

            builder.Services.AddSingleton<Views.AddContactPage>();
            builder.Services.AddTransient<ViewModels.AddContactPageViewModel>();

            builder.Services.AddSingleton<Views.BandsPage>();
            builder.Services.AddTransient<ViewModels.BandsPageViewModel>();

            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<ViewModels.AppShellViewModel>();
           // כאן בוחרים באיזה סרוויס להשתמש

            builder.Services.AddSingleton<IApplicationService, SQLitetService>();
            // builder.Services.AddSingleton<IApplicationService, FakeDBService>();




#if DEBUG
      builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using BH.PAM.StartupHelpers;
using BH.PAM.Data;
using BH.PAM;
using BH.PAM.ViewModel;
using BH.PAM.Services.Interfaces;

namespace BH.PAM;

public partial class App : Application
{
    public static IHost? AppHost { get; private set; }

    public App()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>();
                services.RegisterServices();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        await AppHost!.StartAsync();

        var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
        var unitConversionService = AppHost.Services.GetRequiredService<IUnitConversionService>();
        var dialogService = AppHost.Services.GetRequiredService<IDialogService>();
        MainViewModel vm = new MainViewModel(unitConversionService, dialogService);
        startupForm.DataContext = vm;
        startupForm.Show();
        base.OnStartup(e);
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await AppHost!.StopAsync();
        base.OnExit(e);
    }
}

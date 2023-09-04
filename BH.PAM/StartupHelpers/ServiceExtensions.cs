using BH.PAM.Data;
using BH.PAM.Services;
using BH.PAM.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BH.PAM.StartupHelpers;

public static class ServiceExtensions
{
    public static void AddFormFactory<TForm>(this IServiceCollection services)
        where TForm : class
    {
        services.AddTransient<TForm>();
        services.AddSingleton<Func<TForm>>(x => () => x.GetService<TForm>()!);
        services.AddSingleton<IAbstractFactory<TForm>, AbstractFactory<TForm>>();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IDataAccess, DataAccess>();
        services.AddSingleton<IUnitConversionService, UnitConversionService>();
        services.AddSingleton<IDialogService, DefaultDialogService>();
    }
}

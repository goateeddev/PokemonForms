using Application.Services;
using Application.Services.Ports.Outbound.DataAccess;
using Infrastructure.DataAccess.MySQL;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Windows;
using TradeUB.Infrastructure.DataAccess.Database;

namespace Presentation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var connectionString = $"server=localhost;port=3306;database=pokemonforms;uid=root;pwd=p@ssw0rd;";
            
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddTransient(_ => new MySqlConnection(connectionString));
            serviceDescriptors.AddLogging();
            serviceDescriptors.AddSingleton<MainWindow>();
            serviceDescriptors.AddSingleton<DbClient>();
            serviceDescriptors.AddSingleton<PokemonService>();
            serviceDescriptors.AddSingleton<PokemonApplicationService>();
            serviceDescriptors.AddSingleton<IPokemonDao, PokemonDao>();

            _serviceProvider = serviceDescriptors.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}

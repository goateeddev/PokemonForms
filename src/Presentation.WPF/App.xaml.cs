using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Presentation.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        public App()
        {
            var connectionString = $"server=serverhost;port=3306;database=pokemonforms;uid=app_tradeub;pwd=p@ssw0rd;";
            
            IServiceCollection serviceDescriptors = new ServiceCollection();
            serviceDescriptors.AddTransient(_ => new MySqlConnection(connectionString));
            serviceDescriptors.AddSingleton<MainWindow>();

            _serviceProvider = serviceDescriptors.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}

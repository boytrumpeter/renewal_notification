using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoyalUKInsurance.Renewal.CustomerSevices.CustomerServices;
using RoyalUKInsurance.Renewal.CustomerSevices.Interfaces;
using RoyalUKInsurance.Renewal.RenewalServices.Services;
using RoyalUKInsurance.Renewal.RenewalServices.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RoyalUKInsurance.Renewal.AppUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}|Level:{Level}|ThreadId:{ThreadId}|ContextValue:{ContextValue}|SourceContext:{SourceContext}|Message:{Message}{NewLine}{Exception}";
            //string outTemplate4 = "[{Timestamp:HH:mm:ss:fff} {Level:u3}] {Message:lj}{NewLine}{Exception}"
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(pathFormat: "Logs/log-{Date}.log", restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Debug, fileSizeLimitBytes: 107300074, outputTemplate: outputTemplate)
                .CreateLogger();
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            //base.OnStartup(e);  
        }

        private void ConfigureServices(ServiceCollection serviceCollection )
        {
            serviceCollection.AddScoped<IRenewalMessageService, RenewalMessageService>();
            serviceCollection.AddScoped<ICustomerService, CustomerService>(); 
            serviceCollection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
           
            serviceCollection.AddSingleton<MainWindow>();
            
        }
    }
}

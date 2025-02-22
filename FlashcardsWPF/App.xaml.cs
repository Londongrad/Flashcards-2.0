using Flashcards.Data;
using Flashcards.Extensions;
using Flashcards.ViewModels.UserControls;
using Flashcards.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Flashcards
{
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddPersistence();

            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            ServiceProvider.GetRequiredService<DesignTimeDbContextFactory>().CreateDbContext().Database.EnsureDeleted();
            ServiceProvider.GetRequiredService<DesignTimeDbContextFactory>().CreateDbContext().Database.EnsureCreated();

            ServiceProvider.GetRequiredService<SetsViewModel>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
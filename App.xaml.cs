using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WPF_LoginForm.Data;
using WPF_LoginForm.ViewModels;
using WPF_LoginForm.Views;

namespace WPF_LoginForm;

public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App()
    {        
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDataLayerServices();

        serviceCollection.AddTransient<LoginViewModel>();
        serviceCollection.AddTransient<MainViewModel>();        
        serviceCollection.AddTransient<LoginView>();        
        serviceCollection.AddTransient<MainView>();

        serviceCollection.AddTransient<HomeViewModel>();
        serviceCollection.AddTransient<HomeView>();

        serviceCollection.AddTransient<CustomerViewModel>();
        serviceCollection.AddTransient<CustomerView>();

        Services = serviceCollection.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var loginView = Services.GetRequiredService<LoginView>();
        var loginViewModel = Services.GetRequiredService<LoginViewModel>();
        loginView.DataContext = loginViewModel;
        
        loginViewModel.LoginSuccess += () =>
        {
            var mainView = Services.GetRequiredService<MainView>();
            var mainViewModel = Services.GetRequiredService<MainViewModel>();
            mainView.DataContext = mainViewModel;

            _ = mainViewModel.InitializeWithUserAsync(loginViewModel.Username);


            this.MainWindow = mainView;
            mainView.Show();
            loginView.Close();
        };

        this.MainWindow = loginView;
        loginView.Show();
    }
}


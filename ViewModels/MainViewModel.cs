using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FontAwesome.Sharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WPF_LoginForm.Data;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IDbContextFactory<EfCoreContext> _contextFactory;

    private readonly System.IServiceProvider _serviceProvider;

    [ObservableProperty]
    private UserAccountModel? _currentUserAccount;

    [ObservableProperty]
    private ObservableObject? _currentChildView;

    [ObservableProperty]
    private bool _isBusy;

    // Свойство для отслеживания состояния загрузки в UI
    [ObservableProperty]
    private bool _isDataLoading;

    [ObservableProperty]
    private string _caption;

    [ObservableProperty]
    private IconChar _icon;

    // Свойство-индикатор для UI, чтобы показать «крутилку» (Spinner) во время загрузки
    public Task Initialization { get; }

    public MainViewModel(
        IDbContextFactory<EfCoreContext> contextFactory,
        IServiceProvider serviceProvider)
    {
        _contextFactory = contextFactory;
        _serviceProvider = serviceProvider;

        _currentUserAccount = new UserAccountModel { DisplayName = "Loading profile..." };
        
        NavigateToHome();
    }

    [RelayCommand]
    private void NavigateToHome()
    {
        using var scope = ServiceProviderServiceExtensions.CreateScope(_serviceProvider);
        // Достаем свежую HomeViewModel из DI и делаем её активной
        CurrentChildView = (ObservableObject)_serviceProvider.GetService(typeof(HomeViewModel))!;
        Caption = "Dashboard";
        Icon = IconChar.Home;
    }

    [RelayCommand]
    private void NavigateToCustomer()
    {
        using var scope = ServiceProviderServiceExtensions.CreateScope(_serviceProvider);
        // Достаем свежую CustomerViewModel из DI и делаем её активной
        CurrentChildView = (ObservableObject)_serviceProvider.GetService(typeof(CustomerViewModel))!;
        Caption = "Customers";
        Icon = IconChar.UserGroup;
    }

    [RelayCommand]
    private void NavigateToEmpty()
    {
        // Записываем null — это заставит ContentControl очистить экран
        CurrentChildView = null;
        Caption = "Under Construction";
        Icon = IconChar.Toolbox;
    }


    public async Task InitializeWithUserAsync(string authenticatedUsername)
    {
        if (string.IsNullOrWhiteSpace(authenticatedUsername)) return;

        IsDataLoading = true; // Показываем ProgressBar

        try
        {
            var user = await GetByUsernameNewAsync(authenticatedUsername);

            if (user != null)
            {
                CurrentUserAccount = new UserAccountModel
                {
                    Username = user.Username,
                    DisplayName = $"Welcome {user.Name} {user.LastName} ;)",
                    ProfilePicture = null
                };
            }
            else
            {
                CurrentUserAccount = new UserAccountModel
                {
                    DisplayName = "Invalid user, not logged in"
                };
            }
        }
        catch (Exception)
        {
            // Защита от падения интерфейса при ошибках сети/БД
            CurrentUserAccount = new UserAccountModel
            {
                DisplayName = "Database connection error."
            };
        }
        finally
        {
            IsDataLoading = false;
        }
    }

    private async Task<UserModel?> GetByUsernameNewAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName)) return null;

        using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Username == userName)
            .Select(u => new UserModel
            {
                Id = u.Id,
                Username = u.Username,
                //Password = string.Empty,
                Name = u.Name,
                LastName = u.LastName,
                Email = u.Email
            }).FirstOrDefaultAsync();
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security;
using System.Security.Principal;
using WPF_LoginForm.Data;

namespace WPF_LoginForm.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IDbContextFactory<EfCoreContext> _contextFactory;

    // Событие успешного входа для App.xaml.cs
    public event Action? LoginSuccess;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private string _username = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    private SecureString? _password;    

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    // Кнопка станет активной, только если выполнены все требования к длине строк    
    private bool CanLogin => !string.IsNullOrWhiteSpace(Username) &&
                             Password != null && Password.Length > 0 &&
                             !IsBusy;

    public LoginViewModel(IDbContextFactory<EfCoreContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    partial void OnUsernameChanged(string value) => LoginCommand.NotifyCanExecuteChanged();
    partial void OnPasswordChanged(SecureString? value) => LoginCommand.NotifyCanExecuteChanged();
    partial void OnIsBusyChanged(bool value) => LoginCommand.NotifyCanExecuteChanged();


    [RelayCommand(CanExecute = nameof(CanLogin))]
    private async Task LoginAsync()
    {        
        if (Username.Length < 3)
        {
            ErrorMessage = "* Username must be longer than 2 characters";
            return;
        }
        if (Password.Length < 3)
        {
            ErrorMessage = "* Password must be longer than 2 characters";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        try
        {            
            bool isValidUser = await AuthenticateUser(new NetworkCredential(Username, Password));

            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(Username), null);

                LoginSuccess?.Invoke();
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }            
        }
        catch (Exception)
        {
            ErrorMessage = " * Can't connect to Database.";
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task<bool> AuthenticateUser(NetworkCredential credentials)
    {
        using var context = await _contextFactory.CreateDbContextAsync();
        string targetUsername = credentials.UserName;

        // Ищем пользователя в БД
        var user = await context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == targetUsername);

        if (user == null)
            return false;

        // Извлекаем и сверяем пароль
        string clearTextPassword = credentials.Password;
        bool isValid = user.Password == clearTextPassword;

        // Затираем временную строку в памяти
        clearTextPassword = string.Empty;

        return isValid;

    }


    private void ExecuteRecoverPasswordCommand(string username, string email)
    { 
        throw new NotImplementedException();
    }

}

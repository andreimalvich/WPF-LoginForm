namespace WPF_LoginForm.Models;

public class UserModel
{
    public Guid Id { get; set; } // изменил из string в Guid из-за EFCore
    public string Username { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}

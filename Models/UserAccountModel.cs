namespace WPF_LoginForm.Models;

public class UserAccountModel
{
    public string Username { get; set; } 
    public string DisplayName { get; set; } = "User not logged in";
    public byte[] ProfilePicture { get; set; }
}

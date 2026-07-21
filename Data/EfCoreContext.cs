using Microsoft.EntityFrameworkCore;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Data;

public class EfCoreContext : DbContext
{
    public DbSet<UserModel> Users => Set<UserModel>();

    public EfCoreContext()
    {        
    }

    public EfCoreContext(DbContextOptions<EfCoreContext> options) : base(options)
    {
    }

}

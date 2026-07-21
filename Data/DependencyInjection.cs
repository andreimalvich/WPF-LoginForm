using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WPF_LoginForm.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayerServices(this IServiceCollection services, string connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MVVMLoginDb;Integrated Security=True;Encrypt=False;TrustServerCertificate=True;")
    {
        services.AddDbContextFactory<EfCoreContext>(options => options.UseSqlServer(connectionString));

        return services;
    }
}

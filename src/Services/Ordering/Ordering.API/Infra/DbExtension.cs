using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Ordering.API.Infra;

public static class DbExtension
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder)
        where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();
        // var config = services.GetRequiredService<IConfiguration>().GetConnectionString("OrderingConnectionString");
        // context.Database.SetConnectionString(config);
        try
        {
            logger.LogInformation($"Started Db Migration: {nameof(TContext)}");
            CallSeeder(seeder, context, services);
            logger.LogInformation($"Migration Completed: {nameof(TContext)}");
        }
        catch (SqlException e)
        {
            logger.LogError(e, $"An error occured while migration db: {nameof(TContext)}");
        }
        return host;
    }

    private static void CallSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context, IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
} 
using Microsoft.EntityFrameworkCore;
using Ordering.API.Infra;
using Ordering.Infrastructure.Data;

namespace Ordering.API;

public static class Program
{
    public static void Main(string[] args)
    {
        CreatHostBuilder(args).Build()
            .MigrateDatabase<OrderContext>((ctx, services) =>
            {
                // ctx.Database.SetConnectionString("Server=localhost;Database=OrderDb;User Id=sa;Password=admin1234");
                OrderContextSeed.SeedAsync(ctx, services.GetService<ILogger<OrderContextSeed>>()).Wait();
            })
            .Run();
    }

    private static IHostBuilder CreatHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
            webBuilder.UseStartup<Startup>()
        );
    
}
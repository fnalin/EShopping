using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;

public class OrderContextSeed
{
    public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed>? logger)
    {
        if (!orderContext.Orders.Any())
        {
            orderContext.Orders.AddRange(SetOrders());
            await orderContext.SaveChangesAsync();
            logger?.LogInformation($"Ordering Database seed: {nameof(OrderContext)} seeded successfully");
        }
    }

    private static Order[] SetOrders()
    {
        return new[]
        {
            new Order()
            {
                UserName = "nalin",
                FirstName = "Fabiano",
                LastName = "Nalin",
                EmailAddress = "fabiano.nalin@gmail.com",
                AddressLine = "Rua X, 100 - São Paulo",
                Country = "Brasil",
                TotalPrice = 750,
                State = "SP",
                ZipCode = "05333090",
                
                CardName = "VISA",
                CardNumber = "1234567890123456",
                Expiration = "12/25",
                Cvv = "123",
                PaymentMethod = 1,
                
                CreatedBy = "nalin",
                CreatedDate = DateTime.Now,
                LastModifiedBy = "nalin",
                LastModifiedDate = DateTime.Now
            }
        };
    }
}
using Ordering.Core.Entities;

namespace Ordering.Core.Repositories;

public interface IOrderRepository : IRepository<Order>
{
    Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string username);
}
using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Infrastructure.Data.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext orderContext) : base(orderContext)
    {
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserNameAsync(string username)
    {
        return await _dbContext.Set<Order>().Where(o => o.UserName == username).ToListAsync();
    }
}
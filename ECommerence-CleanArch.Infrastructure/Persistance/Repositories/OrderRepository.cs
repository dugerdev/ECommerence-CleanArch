using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Domain.Common.Enums;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

   
    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(
        Guid customerId,  // DÜZELTME: int → Guid
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.CustomerId == customerId && !o.IsDeleted)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderWithItemsAsync(
        Guid orderId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.Id == orderId && !o.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(
        OrderStatus status, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.OrderStatus == status && !o.IsDeleted)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetOrderByOrderNumberAsync(
        string orderNumber, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber && !o.IsDeleted, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(
        DateTime startDate, 
        DateTime endDate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(o => o.OrderItems)
            .Where(o => o.OrderDate >= startDate 
                     && o.OrderDate <= endDate 
                     && !o.IsDeleted)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync(cancellationToken);
    }
}

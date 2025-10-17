using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories;

public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
{
    public OrderItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Siparişe ait tüm kalemleri getir
    public async Task<IEnumerable<OrderItem>> GetByOrderIdAsync(
        Guid orderId,  // ← Guid (OrderItem entity'de Guid)
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(oi => oi.Product)
            .Where(oi => oi.OrderId == orderId && !oi.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    // Ürünün satış geçmişini getir
    public async Task<IEnumerable<OrderItem>> GetByProductByIdAsync(
        Guid productId,  // ← Guid (OrderItem entity'de Guid)
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(oi => oi.Order)
            .Where(oi => oi.ProductId == productId && !oi.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}

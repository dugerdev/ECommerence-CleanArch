using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    public ShoppingCartRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Müşterinin sepetini getir
    public async Task<ShoppingCart?> GetByCustomerIdAsync(
        Guid customerId,  // ← Guid (ShoppingCart entity'de Guid)
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(sc => sc.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(
                sc => sc.CustomerId == customerId && !sc.IsDeleted, 
                cancellationToken);
    }

    // Sepeti kalemleriyle birlikte getir
    public async Task<ShoppingCart?> GetCartWithItemsAsync(
        Guid cartId, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(sc => sc.CartItems)
            .ThenInclude(ci => ci.Product)
            .Include(sc => sc.Customer)
            .FirstOrDefaultAsync(
                sc => sc.Id == cartId && !sc.IsDeleted, 
                cancellationToken);
    }

    // Terk edilmiş sepetleri getir
    public async Task<IEnumerable<ShoppingCart>> GetAbandonedCartsAsync(
        int daysOld, 
        CancellationToken cancellationToken = default)
    {
        var cutoffDate = DateTimeOffset.UtcNow.AddDays(-daysOld);
        
        return await _dbSet
            .Include(sc => sc.CartItems)
            .Where(sc => sc.UpdatedAt < cutoffDate && !sc.IsDeleted)
            .ToListAsync(cancellationToken);
    }
}


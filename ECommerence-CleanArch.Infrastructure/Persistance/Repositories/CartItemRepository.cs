using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories;

public class CartItemRepository : Repository<CartItem>, ICartItemRepository
{
    public CartItemRepository(ApplicationDbContext context) : base(context)
    {
    }

    // Sepetteki tüm kalemleri getir
    public async Task<IEnumerable<CartItem>> GetByCartIdAsync(
        Guid cartId,  // ← Guid (CartItem entity'de ShoppingCartId Guid)
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(ci => ci.Product)
            .Where(ci => ci.ShoppingCartId == cartId && !ci.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    // Sepette bu ürün var mı kontrol et
    public async Task<CartItem?> GetByCartAndProductAsync(
        Guid cartId,  // ← Guid (CartItem entity'de ShoppingCartId Guid)
        Guid productId,  // ← Guid (CartItem entity'de ProductId Guid)
        CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(ci => ci.Product)
            .FirstOrDefaultAsync(
                ci => ci.ShoppingCartId == cartId 
                   && ci.ProductId == productId 
                   && !ci.IsDeleted, 
                cancellationToken);
    }
}


using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Domain.Interfaces;

public interface ICartItemRepository : IRepository<CartItem>
{
    // DÜZELTME: int → Guid (CartItem entity'de ShoppingCartId ve ProductId artık Guid)
    Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid cartId, CancellationToken cancellationToken = default);
    Task<CartItem?> GetByCartAndProductAsync(Guid cartId, Guid productId, CancellationToken cancellationToken = default);
}

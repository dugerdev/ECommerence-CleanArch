using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Domain.Interfaces;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    // DÜZELTME: int → Guid (ShoppingCart entity'de CustomerId artık Guid)
    Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<ShoppingCart?> GetCartWithItemsAsync(Guid cartId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ShoppingCart>> GetAbandonedCartsAsync(int daysOld, CancellationToken cancellationToken = default);
}

using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Application.Contracts.Repositories;

public interface CartItemRepository : IAsyncRepository<CartItem>
{
    Task<IEnumerable<CartItem>> GetByShoppingCartIdAsync(Guid shoppingCartId, CancellationToken cancellationToken = default);

    Task<IEnumerable<CartItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

    Task<Paginate<CartItem>> GetPagedByShoppingCartIdAsync(
       Guid shoppingCartId,
       int index = 0,
       int size = 10,
       CancellationToken cancellationToken = default);
}

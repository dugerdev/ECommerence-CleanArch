using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Application.Contracts.Repositories;

public interface IShoppingCartRepository : IAsyncRepository<ShoppingCart>
{
    Task<ShoppingCart?> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);

    Task<IEnumerable<ShoppingCart>> GetActiveCartsAsync(CancellationToken cancellationToken = default);

    Task<Paginate<ShoppingCart>> GetPagedByCustomerAsync(
        Guid customerId,
        int index = 0,
        int size = 10,
        CancellationToken cancellationToken = default);
}

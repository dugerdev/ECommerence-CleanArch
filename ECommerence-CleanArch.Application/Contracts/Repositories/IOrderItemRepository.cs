using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Application.Contracts.Repositories
{
    public interface IOrderItemRepository : IAsyncRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);

        Task<IEnumerable<OrderItem>> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);

        Task<Paginate<OrderItem>> GetPagedByOrderIdAsync(
        Guid orderId,
        int index = 0,
        int size = 10,
        CancellationToken cancellationToken = default);

    }
}

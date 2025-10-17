using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Domain.Interfaces;

public interface IOrderItemRepository : IRepository<OrderItem>
{
    // DÜZELTME: int → Guid (OrderItem entity'de OrderId ve ProductId artık Guid)
    Task<IEnumerable<OrderItem>> GetByOrderIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderItem>> GetByProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}

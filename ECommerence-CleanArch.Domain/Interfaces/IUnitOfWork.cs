namespace ECommerence_CleanArch.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repository Properties
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    ICustomerRepository Customers { get; }
    IOrderRepository Orders { get; }
    IOrderItemRepository OrderItems { get; }
    IShoppingCartRepository ShoppingCarts { get; }
    ICartItemRepository CartItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}

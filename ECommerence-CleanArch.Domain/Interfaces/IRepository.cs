using ECommerence_CleanArch.Domain.Common;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Domain.Interfaces;

public interface IRepository<T> where T : EntityBase
{
    // Query Operations
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    // Command Operations
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

    // Soft Delete
    Task SoftDeleteAsync(Guid id, CancellationToken cancellationToken = default);

    // Check Existence
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
}

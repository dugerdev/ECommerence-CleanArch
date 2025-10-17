using ECommerence_CleanArch.Domain.Entity;

namespace ECommerence_CleanArch.Domain.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Customer?> GetCustomerWithOrdersAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country, CancellationToken cancellationToken = default);
    Task<bool> IsEmailUniqueAsync(string email , CancellationToken cancellationToken = default);
}

using Microsoft.EntityFrameworkCore;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Interfaces;
using ECommerence_CleanArch.Infrastructure.Persistance.Context;

namespace ECommerence_CleanArch.Infrastructure.Persistance.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .FirstOrDefaultAsync(
                c => c.Email == email && !c.IsDeleted, 
                cancellationToken);
    }

    public async Task<Customer?> GetCustomerWithOrdersAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(c => c.Orders)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(
                c => c.Id == id && !c.IsDeleted, 
                cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(c => c.Country == country && !c.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await _dbSet.AnyAsync(
            c => c.Email == email && !c.IsDeleted, 
            cancellationToken);
    }
}

using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.DTOs.Customer;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Services;

/// <summary>
/// Customer service interface
/// Business logic için contract
/// </summary>
public interface ICustomerService
{
    Task<Paginate<CustomerDto>> GetListAsync(
        Expression<Func<Customer, bool>>? predicate = null,
        Func<IQueryable<Customer>, IOrderedQueryable<Customer>>? orderBy = null,
        Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<CustomerDto?> GetAsync(
        Expression<Func<Customer, bool>> predicate,
        Func<IQueryable<Customer>, IIncludableQueryable<Customer, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<bool> AnyAsync(
        Expression<Func<Customer, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<CustomerDto> AddAsync(Customer entity);
    Task<CustomerDto> UpdateAsync(Customer entity);
    Task<CustomerDto> DeleteAsync(Customer entity, bool permanent = false);

    Task<CustomerDto?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerDto>> GetActiveCustomersAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerDto>> GetCustomersByCityAsync(string city, CancellationToken cancellationToken = default);
}
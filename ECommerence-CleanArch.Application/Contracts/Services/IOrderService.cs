using ECommerence_CleanArch.Application.Common;
using ECommerence_CleanArch.Application.DTOs.Order;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Contracts.Services;

/// <summary>
/// Order service interface
/// Business logic için contract
/// </summary>
public interface IOrderService
{
    Task<Paginate<OrderDto>> GetListAsync(
        Expression<Func<Order, bool>>? predicate = null,
        Func<IQueryable<Order>, IOrderedQueryable<Order>>? orderBy = null,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<OrderDto?> GetAsync(
        Expression<Func<Order, bool>> predicate,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<bool> AnyAsync(
        Expression<Func<Order, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default
        );

    Task<OrderDto> AddAsync(Order entity);
    Task<OrderDto> UpdateAsync(Order entity);
    Task<OrderDto> DeleteAsync(Order entity, bool permanent = false);

    Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default);
    Task<OrderDto?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);
}
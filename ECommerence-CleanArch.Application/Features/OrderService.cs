using AutoMapper;
using ECommerence_CleanArch.Application.Contracts.Repositories;
using ECommerence_CleanArch.Application.Contracts.Services;
using ECommerence_CleanArch.Application.DTOs.Order;
using ECommerence_CleanArch.Application.Paging;
using ECommerence_CleanArch.Domain.Entity;
using ECommerence_CleanArch.Domain.Common.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace ECommerence_CleanArch.Application.Features;

/// <summary>
/// Order service implementation
/// Business logic + Repository coordination
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<Paginate<OrderDto>> GetListAsync(
        Expression<Func<Order, bool>>? predicate = null,
        Func<IQueryable<Order>, IOrderedQueryable<Order>>? orderBy = null,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var pagedOrders = await _orderRepository.GetListAsync(
            predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);

        return new Paginate<OrderDto>
        {
            Index = pagedOrders.Index,
            Size = pagedOrders.Size,
            Count = pagedOrders.Count,
            Pages = pagedOrders.Pages,
            Items = _mapper.Map<IList<OrderDto>>(pagedOrders.Items)
        };
    }

    public async Task<OrderDto?> GetAsync(
        Expression<Func<Order, bool>> predicate,
        Func<IQueryable<Order>, IIncludableQueryable<Order, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }

    public async Task<bool> AnyAsync(
        Expression<Func<Order, bool>>? predicate = null,
        bool withDeleted = false,
        bool enableTracking = false,
        CancellationToken cancellationToken = default)
    {
        return await _orderRepository.AnyAsync(predicate, withDeleted, enableTracking, cancellationToken);
    }

    public async Task<OrderDto> AddAsync(Order entity)
    {
        var addedOrder = await _orderRepository.AddAsync(entity);
        return _mapper.Map<OrderDto>(addedOrder);
    }

    public async Task<OrderDto> UpdateAsync(Order entity)
    {
        var updatedOrder = await _orderRepository.UpdateAsync(entity);
        return _mapper.Map<OrderDto>(updatedOrder);
    }

    public async Task<OrderDto> DeleteAsync(Order entity, bool permanent = false)
    {
        var deletedOrder = await _orderRepository.DeleteAsync(entity, permanent);
        return _mapper.Map<OrderDto>(deletedOrder);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId, cancellationToken);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(OrderStatus status, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetOrdersByStatusAsync(status, cancellationToken);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTimeOffset startDate, DateTimeOffset endDate, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate, cancellationToken);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, cancellationToken);
        return order != null ? _mapper.Map<OrderDto>(order) : null;
    }
}

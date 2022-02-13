using AutoMapper;
using ECommerce.Api.Orders.Db;
using ECommerce.Api.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Orders.Providers;

public class OrdersProvider : IOrdersProvider
{
    private OrdersDbContext _dbContext;
    private ILogger<OrdersProvider> _logger;
    private IMapper _mapper;

    public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;

        SeedData();
    }

    private void SeedData()
    {
        if (_dbContext.Orders.Any()) return;
        _dbContext.Orders.AddRange(
            new List<Order>
            {
                new()
                {
                    Id = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-1), Total = (decimal)1.0,
                    Items = new List<Db.OrderItem>
                    {
                        new() { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = (decimal)1.0 }
                    }
                },
                new()
                {
                    Id = 2, CustomerId = 2, OrderDate = DateTime.Now.AddDays(-2), Total = (decimal)4.0,
                    Items = new List<OrderItem>
                    {
                        new() { Id = 2, OrderId = 2, ProductId = 2, Quantity = 2, UnitPrice = (decimal)2.0 }
                    }
                },
                new()
                {
                    Id = 3, CustomerId = 3, OrderDate = DateTime.Now.AddDays(-3), Total = (decimal)9.0,
                    Items = new List<OrderItem>
                    {
                        new() { Id = 3, OrderId = 3, ProductId = 3, Quantity = 3, UnitPrice = (decimal)3.0 }
                    }
                }
            });
        _dbContext.SaveChanges();
    }

    public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
    {
        try
        {
            var orders = await _dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
            if (orders.Any())
            {
                var result = _mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                return (true, result, null);
            }

            return (false, null, "Not found");
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return (false, null, e.Message);
        }
    }
}
using AutoMapper;
using ECommerce.Api.Customers.Db;
using ECommerce.Api.Customers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.Customers.Providers;

public class CustomersProvider : ICustomersProvider
{
    private CustomersDbContext _dbContext;
    private ILogger<CustomersProvider> _logger;
    private readonly IMapper _mapper;

    public CustomersProvider(CustomersDbContext dbContext, ILogger<CustomersProvider> logger, IMapper mapper)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;

        SeedData();
    }

    private void SeedData()
    {
        if (_dbContext.Customers.Any()) return;
        _dbContext.Customers.AddRange(
            new List<Db.Customer>
            {
                new() { Id = 1, Name = "Jimmy Johnes", Address = "Neverly 1313, Neverland" },
                new() { Id = 2, Name = "Bill Bobich", Address = "Anderly 7171, Azure" },
                new() { Id = 3, Name = "Sumbrella Joharova", Address = "Peeply 1313, Palps" }
            });
        _dbContext.SaveChanges();
    }

    public async Task<(bool IsSuccess, IEnumerable<Models.Customer> Customers, string ErrorMessage)> GetCustomersAsync()
    {
        try
        {
            var customers = await _dbContext.Customers.ToListAsync();
            if (customers.Any())
            {
                var result = _mapper.Map<IEnumerable<Customer>, IEnumerable<Models.Customer>>(customers);
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

    public async Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id)
    {
        try
        {
            var customer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
            if (customer != null)
            {
                var result = _mapper.Map<Customer, Models.Customer>(customer);
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
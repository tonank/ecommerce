using System.Text.Json;
using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Models;

namespace ECommerce.Api.Search.Services;

public class ProductsService : IProductsService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<IProductsService> _logger;

    public ProductsService(IHttpClientFactory _httpClientFactory, ILogger<IProductsService> logger)
    {
        this._httpClientFactory = _httpClientFactory;
        _logger = logger;
    }
    
    public async Task<(bool IsSuccess, IEnumerable<Product> Products, string ErrorMessage)> GetProductsAsync()
    {
        try
        {
            var client = _httpClientFactory.CreateClient("ProductsService");
            var response = await client.GetAsync("api/products");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var result = JsonSerializer.Deserialize<IEnumerable<Product>>(content, options);
                return (true, result, null);
            }

            return (false, null, response.ReasonPhrase);
        }
        catch (Exception e)
        {
            _logger.LogError(e.ToString());
            return (false, null, e.Message);
        }
    }
}
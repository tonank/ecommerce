﻿using ECommerce.Api.Orders.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Orders.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private IOrdersProvider _ordersProvider;

    public OrdersController(IOrdersProvider ordersProvider)
    {
        _ordersProvider = ordersProvider;
    }
    
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetOrders(int customerId)
    {
        var result = await _ordersProvider.GetOrdersAsync(customerId);
        if (result.IsSuccess)
        {
            return Ok(result.Orders);
        }

        return NotFound();
    }
}
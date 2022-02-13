﻿using ECommerce.Api.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Customers.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private ICustomersProvider _customersProvider;

    public CustomersController(ICustomersProvider customersProvider)
    {
        _customersProvider = customersProvider;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync()
    {
        var result = await _customersProvider.GetCustomersAsync();
        if (result.IsSuccess)
        {
            return Ok(result.Customers);   
        }

        return NotFound();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var result = await _customersProvider.GetCustomerAsync(id);
        if (result.IsSuccess)
        {
            return Ok(result.Customer);
        }

        return NotFound();
    }
}
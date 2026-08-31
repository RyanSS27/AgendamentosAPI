using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Domain.Services;
using AgendamentosAPI.Dtos;
using AgendamentosAPI.Dtos.Customer;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosAPI.Adapters.Controllers;

[ApiController]
[Route("/customers")]
public class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddCustomer(CustomerInputDto input)
    {
        var customer = await customerService.AddCustomerAsync(input);
        
        return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        return Ok(await customerService.GetCustomerByIdAsync(id)); 
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCustomer(Guid id, CustomerInputDto input)
    {
        return Ok(await customerService.UpdateCustomerAsync(id, input)); 
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        await customerService.DeleteCustomerAsync(id);
        
        return NoContent(); 
    }

    [HttpGet("list/{limit:int}")]
    public async Task<IActionResult> ListCustomers(int? limit)
    {
        return Ok(await customerService.ListCustomersAsync(limit));
    }
}
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Domain.Services;
using AgendamentosAPI.Dtos;
using AgendamentosAPI.Dtos.ServiceProvider;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosAPI.Adapters.Controllers;

[ApiController]
[Route("api/service-provider")]
public class ServiceProviderController(IServiceProviderService serviceProviderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddServiceProver(ServiceProviderInputDto input)
    {
        var provider = await serviceProviderService.AddServiceProviderAsync(input);
        
        return CreatedAtAction(nameof(GetServiceProvider), new { id = provider.Id }, provider);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetServiceProvider(Guid id)
    {
        return Ok(await serviceProviderService.GetServiceProviderByIdAsync(id));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateServiceProvider(Guid id, ServiceProviderInputDto input)
    {
        return Ok(await serviceProviderService.UpdateServiceProviderAsync(id, input));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteServiceProvider(Guid id)
    {
        await serviceProviderService.DeleteServiceProviderAsync(id);
        return NoContent();
    }

    [HttpGet("list/{limit:int}")]
    public async Task<IActionResult> ListCustomers(int? limit)
    {
        return Ok(await serviceProviderService.ListServiceProvidersAsync(limit));
    }
}
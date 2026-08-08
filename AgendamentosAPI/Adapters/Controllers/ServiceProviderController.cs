using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosAPI.Adapters.Controllers;

[ApiController]
[Route("api/service-provider")]
public class ServiceProviderController(IServiceProviderService serviceProviderService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddServiceProver(ServiceProviderInputDto provider)
    {
        return Ok(await serviceProviderService.AddServiceProviderAsync(provider));
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetServiceProvider(Guid id)
    {
        return Ok(await serviceProviderService.GetServiceProviderByIdAsync(id));
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> UpdateServiceProvider(Guid id, ServiceProviderInputDto provider)
    {
        return Ok(await serviceProviderService.UpdateServiceProviderAsync(id, provider));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteServiceProvider(Guid id)
    {
        await serviceProviderService.DeleteServiceProviderAsync(id);
        return NoContent();
    }
}
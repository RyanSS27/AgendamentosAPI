using AgendamentosAPI.Domain.Ports.ServicePorts;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosAPI.Adapters.Controllers;

[ApiController]
[Route("/appointment")]
public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet("{providerId:Guid}")]
    public async Task<IActionResult> GetFreePeriodsSlots(Guid providerId, DateOnly start, DateOnly end, TimeSpan duration)
    {
        return Ok(await appointmentService.GetFreePeriodsSlotsAsync(providerId, start, end, duration));
    }
}
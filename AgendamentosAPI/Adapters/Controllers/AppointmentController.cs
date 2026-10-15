using AgendamentosAPI.Adapters.Controllers.Requests;
using AgendamentosAPI.Domain.Ports.ServicePorts;
using AgendamentosAPI.Dtos.Appointments.Filters;
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

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] SearchAppointmentsRequest request)
    {
        var filter = new AppointmentFilter(
            ProviderId: request.ProviderId,
            CustomerId: request.CustomerId,
            Status: request.Status,
            PeriodStart: request.PeriodStart,
            PeriodEnd: request.PeriodEnd,
            Page: request.Page,
            PageSize: request.PageSize
        );

        return Ok(await appointmentService.SearchAppointmentAsync(filter));
    } 
}
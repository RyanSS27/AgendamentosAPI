using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Ports;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentosAPI.Adapters.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CalendarTestController : ControllerBase
{
    private readonly ICalendarIntegrationPort _calendarPort;

    public CalendarTestController(ICalendarIntegrationPort calendarPort)
    {
        _calendarPort = calendarPort;
    }

    [HttpGet("busy-periods")]
    public async Task<IActionResult> GetBusyPeriods([FromQuery] string calendarId)
    {
        // Definimos uma janela de tempo de 7 dias a partir de agora para o teste
        var start = DateTimeOffset.UtcNow;
        var end = start.AddDays(7);

        try
        {
            var busyPeriods = await _calendarPort.GetBusyPeriodsAsync(calendarId, start, end);
            
            return Ok(new 
            { 
                calendarId, 
                start, 
                end, 
                busyPeriods 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = ex.Message, stack = ex.StackTrace });
        }
    }
    
    [HttpPost("events")]
    public async Task<IActionResult> CreateEvent([FromQuery] string calendarId)
    {
        // Cria um agendamento fictício para amanhã
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var end = start.AddHours(1);
        
        var dummyAppointment = new Appointment(
            Guid.NewGuid(), Guid.NewGuid(), start, end, "Consulta Sandbox - Criação");

        try
        {
            var eventId = await _calendarPort.CreateEventAsync(calendarId, dummyAppointment);
            return Ok(new { Mensagem = "Criado com sucesso", ExternalEventId = eventId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = ex.Message, stack = ex.StackTrace });
        }
    }

    [HttpPatch("events/{eventId}")]
    public async Task<IActionResult> UpdateEvent([FromQuery] string calendarId, string eventId)
    {
        // Cria um agendamento fictício remarcado para daqui a 2 dias
        var newStart = DateTimeOffset.UtcNow.AddDays(2);
        var newEnd = newStart.AddHours(1);
        
        var dummyAppointment = new Appointment(
            Guid.NewGuid(), Guid.NewGuid(), newStart, newEnd, "Consulta Sandbox - Atualização");
            
        dummyAppointment.LinkExternalEvent(eventId);

        try
        {
            await _calendarPort.UpdateEventAsync(calendarId, dummyAppointment);
            return Ok(new { Mensagem = "Atualizado com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = ex.Message, stack = ex.StackTrace });
        }
    }

    [HttpDelete("events/{eventId}")]
    public async Task<IActionResult> DeleteEvent([FromQuery] string calendarId, string eventId)
    {
        try
        {
            await _calendarPort.CancelEventAsync(calendarId, eventId);
            return Ok(new { Mensagem = "Deletado com sucesso" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { erro = ex.Message, stack = ex.StackTrace });
        }
    }
}
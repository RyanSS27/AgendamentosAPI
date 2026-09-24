using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos.Appointment;
using AgendamentosAPI.Dtos.Appointment.Filters;

namespace AgendamentosAPI.Domain.Ports.ServicePorts;

public interface IAppointmentService
{
    Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsSlotsAsync(Guid providerId, DateOnly start, DateOnly end);
    Task<FreeSlotsResponse> GetFreePeriodsSlotsAsync(Guid providerId, DateOnly start, DateOnly end, TimeSpan duration);
    Task<AppointmentOutDto> CreateEventAsync(string calendarId, Appointment appointment);
    Task<AppointmentOutDto> UpdateEventAsync(string calendarId, Appointment appointment);
    Task<AppointmentOutDto> CancelEventAsync(string calendarId, string externalEventId);
    Task<PagedResult<Appointment>> SearchAppointmentAsync(AppointmentFilter filter);
    
    // Apenas para testes 
    Task DeleteAppointmentAsync(Guid appointmentId);
}

public record TimePeriod(DateTimeOffset Start, DateTimeOffset End);
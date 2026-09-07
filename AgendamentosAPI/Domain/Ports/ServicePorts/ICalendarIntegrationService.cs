using AgendamentosAPI.Domain.Entities;

namespace AgendamentosAPI.Domain.Ports.ServicePorts;

// Porta de comunicação com o serviço da API externa
public interface ICalendarIntegrationService
{
    Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsAsync(string calendarId, DateTimeOffset start, DateTimeOffset end);
    Task<string> CreateEventAsync(string calendarId, Appointment appointment);
    Task UpdateEventAsync(string calendarId, Appointment appointment);
    Task CancelEventAsync(string calendarId, string externalEventId);
}

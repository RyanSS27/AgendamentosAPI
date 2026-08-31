using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos;
using AgendamentosAPI.Dtos.Appointment;
using Google.Apis.Calendar.v3.Data;

namespace AgendamentosAPI.Domain.Ports;

// Porta de comunicação com o serviço da API externa
public interface ICalendarIntegrationPort
{
    Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsAsync(string calendarId, DateTimeOffset start, DateTimeOffset end);
    Task<string> CreateEventAsync(string calendarId, Appointment appointment);
    Task UpdateEventAsync(string calendarId, Appointment appointment);
    Task CancelEventAsync(string calendarId, string externalEventId);
}

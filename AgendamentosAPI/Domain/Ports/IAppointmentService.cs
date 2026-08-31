using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos.Appointment;

namespace AgendamentosAPI.Domain.Ports;

public interface IAppointmentService
{
    // retorna blocos com os horários ocupados onde ocorrem conflito
    Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsSlotsAsync(Guid providerId, DateTimeOffset start, DateTimeOffset end, TimeSpan duration);
    Task<AppointmentOutDto> CreateEventAsync(string calendarId, Appointment appointment);
    Task<AppointmentOutDto> UpdateEventAsync(string calendarId, Appointment appointment);
    Task<AppointmentOutDto> CancelEventAsync(string calendarId, string externalEventId);
}

public record TimePeriod(DateTimeOffset Start, DateTimeOffset End);
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos.Appointment;

namespace AgendamentosAPI.Domain.Ports.ServicePorts;

public interface IAppointmentService
{
    // retorna blocos com os horários ocupados onde ocorrem conflito
    // optei por alterar a busca, fazendo-a ser por dias, logo, devo alterar a assinatura do método
    Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsSlotsAsync(Guid providerId, DateOnly start, DateOnly end);
    Task<FreeSlotsResponse> GetFreePeriodsSlotsAsync(Guid providerId, DateOnly start, DateOnly end, TimeSpan duration);
    Task<AppointmentOutDto> CreateEventAsync(string calendarId, Appointment appointment);
    Task<AppointmentOutDto> UpdateEventAsync(string calendarId, Appointment appointment);
    Task<AppointmentOutDto> CancelEventAsync(string calendarId, string externalEventId);
}

public record TimePeriod(DateTimeOffset Start, DateTimeOffset End);
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos.Appointment;

namespace AgendamentosAPI.Domain.Services;

public class AppointmentService(IServiceProvider providerService, ICustomerService customerService) : IAppointmentService 
{
    public Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsSlotsAsync(Guid providerId, DateTimeOffset start, DateTimeOffset end, TimeSpan duration)
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentOutDto> CreateEventAsync(string calendarId, Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentOutDto> UpdateEventAsync(string calendarId, Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentOutDto> CancelEventAsync(string calendarId, string externalEventId)
    {
        throw new NotImplementedException();
    }
}
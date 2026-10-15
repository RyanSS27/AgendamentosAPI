using AgendamentosAPI.Domain.Entities;

namespace AgendamentosAPI.Dtos.Appointments;

public record AppointmentOutDto(
    Guid Id,
    Guid ProviderId,
    Guid CustomerId,
    DateTimeOffset Start,
    DateTimeOffset End,
    string Title,
    string Status, 
    string? Observations,
    string? ExternalEventId
)
{
    public static AppointmentOutDto FromEntity(Appointment appointment)
    {
        return new AppointmentOutDto(
            appointment.Id,
            appointment.ProviderId,
            appointment.CustomerId,
            appointment.Start,
            appointment.End,
            appointment.AppointmentTitle,
            appointment.Status.ToString(), 
            appointment.Observations,
            appointment.ExternalEventId
        );
    }
}
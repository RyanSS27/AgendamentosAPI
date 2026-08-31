using AgendamentosAPI.Domain.Entities.Enums;

namespace AgendamentosAPI.Dtos.Appointment;

public record AppointmentInputDto(
    Guid ProviderId,
    Guid CustomerId,
    DateTimeOffset Start,
    DateTimeOffset End,
    AppointmentStatus Status,
    string? Observations
    );
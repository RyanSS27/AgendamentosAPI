using AgendamentosAPI.Domain.Entities.Enums;

namespace AgendamentosAPI.Dtos.Appointments;

public record AppointmentInputDto(
    Guid ProviderId,
    Guid CustomerId,
    DateTimeOffset Start,
    DateTimeOffset End,
    string Title,
    AppointmentStatus Status,
    string? Observations
    );
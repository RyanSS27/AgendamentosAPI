using AgendamentosAPI.Domain.Entities.Enums;

namespace AgendamentosAPI.Dtos.Appointment.Filters;

public record AppointmentFilter(
    Guid? ProviderId = null,
    Guid? CustomerId = null,
    AppointmentStatus? Status = null,
    DateTimeOffset? PeriodStart = null,
    DateTimeOffset? PeriodEnd = null,
    int Page = 1,
    int PageSize = 25 // Valor padrão
);
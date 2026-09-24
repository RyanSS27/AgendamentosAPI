using System.ComponentModel.DataAnnotations;
using AgendamentosAPI.Domain.Entities.Enums;

namespace AgendamentosAPI.Adapters.Controllers.Requests;

public record SearchAppointmentsRequest
{
    public Guid? ProviderId { get; init; }
    public Guid? CustomerId { get; init; }
    public AppointmentStatus? Status { get; init; }
    
    public DateTimeOffset? PeriodStart { get; init; }
    public DateTimeOffset? PeriodEnd { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior ou igual a 1.")]
    public int Page { get; init; } = 1;

    [Range(1, 25, ErrorMessage = "O tamanho da página deve ser no máximo 25.")]
    public int PageSize { get; init; } = 25;
}
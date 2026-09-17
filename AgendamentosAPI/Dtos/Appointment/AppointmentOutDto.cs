using AgendamentosAPI.Domain.Entities.Enums;

namespace AgendamentosAPI.Dtos.Appointment;

public record AppointmentOutDto(
    Guid Id,
    Guid ProviderId,
    Guid CustomerId,
    string ExternalCalendarId, 
        
    DateTimeOffset Start,
    DateTimeOffset End,
    DateTimeOffset CreatedAt, 
        
    string AppointmentTitle,
    AppointmentStatus Status, 
    string? Observations  
    );
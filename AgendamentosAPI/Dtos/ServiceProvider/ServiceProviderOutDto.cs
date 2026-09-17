namespace AgendamentosAPI.Dtos.ServiceProvider;

public record ServiceProviderOutDto(
    Guid Id,
    string Name,
    string Cpf,
    string? Email,
    string? CalendarId,
    TimeOnly WorkStartTime,
    TimeOnly WorkEndTime,
    bool IsOvernightShift,
    bool IsActive
    );
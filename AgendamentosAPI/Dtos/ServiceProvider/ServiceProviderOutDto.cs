namespace AgendamentosAPI.Dtos.ServiceProvider;

public record ServiceProviderOutDto(
    Guid Id,
    string Name,
    string? Email,
    TimeOnly WorkStartTime,
    TimeOnly WorkEndTime,
    bool IsOvernightShift,
    bool IsActive
    );
namespace AgendamentosAPI.Dtos;

public record ServiceProviderOutDto(
    Guid Id,
    string Name,
    string? Email,
    bool IsActive
    );
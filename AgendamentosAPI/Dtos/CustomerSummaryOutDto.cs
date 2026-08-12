namespace AgendamentosAPI.Dtos;

public record CustomerSummaryOutDto(Guid Id, string Name, string Cpf, bool IsActive);   
namespace AgendamentosAPI.Dtos.Customer;

public record CustomerSummaryOutDto(Guid Id, string Name, string Cpf, bool IsActive);   
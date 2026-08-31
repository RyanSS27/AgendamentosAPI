namespace AgendamentosAPI.Dtos.Customer;

public record CustomerOutDto(
    Guid Id, 
    string Name, 
    string Cpf, 
    IReadOnlyCollection<string> PhoneNumbers, 
    bool IsActive
);
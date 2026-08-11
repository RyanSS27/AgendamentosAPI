using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Domain.Ports;

public interface ICustomerService
{
    Task <CustomerOutDto>  AddCustomerAsync(CustomerInputDto input);
    Task<CustomerOutDto> GetCustomerByIdAsync(Guid id);
    Task<CustomerOutDto> UpdateCustomerAsync(Guid id, CustomerInputDto input);
    Task<List<CustomerOutDto>> ListCustomersAsync();
    
    // Apenas para testes:
    Task DeleteCustomerAsync(Guid id);
}
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories.Ports;

public interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    Task UpdateCustomerAsync(Customer customer);
    Task<CustomerOutDto> ListCustomersAsync();
    
    // Apenas para testes:
    Task DeleteCustomerAsync(Customer customer);
}
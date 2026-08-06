using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Domain.Ports;

public interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);
    Task<Customer> GetCustomerByIdAsync(Guid id);
    Task UpdateCustomerAsync(Customer customer);
    Task<CustomerOutDto> ListCustomersAsync();
    
    // Apenas para testes:
    Task DeletCustomerAsync(Guid id);
}
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos;
using AgendamentosAPI.Dtos.Customer;

namespace AgendamentosAPI.Domain.Ports;

public interface ICustomerRepository
{
    Task AddCustomerAsync(Customer customer);
    Task<Customer?> GetCustomerByIdAsync(Guid id);
    Task UpdateCustomerAsync(Customer customer);
    Task<List<CustomerSummaryOutDto>> ListCustomersAsync(int limit);
    
    // Apenas para testes:
    Task DeleteCustomerAsync(Customer customer);
}
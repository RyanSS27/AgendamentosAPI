using AgendamentosAPI.Dtos.Customer;

namespace AgendamentosAPI.Domain.Services;

public interface ICustomerService
{
    Task <CustomerOutDto>  AddCustomerAsync(CustomerInputDto customerInput);
    Task<CustomerOutDto> GetCustomerByIdAsync(Guid id);
    Task<CustomerOutDto> UpdateCustomerAsync(Guid id, CustomerInputDto customerInput);
    Task<List<CustomerSummaryOutDto>> ListCustomersAsync(int? limit);
    
    // Apenas para testes:
    Task DeleteCustomerAsync(Guid id);
}
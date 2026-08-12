using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Domain.Services;

public interface ICustomerService
{
    Task <CustomerOutDto>  AddCustomerAsync(CustomerInputDto input);
    Task<CustomerOutDto> GetCustomerByIdAsync(Guid id);
    Task<CustomerOutDto> UpdateCustomerAsync(Guid id, CustomerInputDto input);
    Task<List<CustomerSummaryOutDto>> ListCustomersAsync(int? limit);
    
    // Apenas para testes:
    Task DeleteCustomerAsync(Guid id);
}
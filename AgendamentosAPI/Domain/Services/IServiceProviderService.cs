using AgendamentosAPI.Dtos.ServiceProvider;

namespace AgendamentosAPI.Domain.Services;

public interface IServiceProviderService
{
    // CRUD
    Task <ServiceProviderOutDto>  AddServiceProviderAsync(ServiceProviderInputDto providerInput);
    Task<ServiceProviderOutDto> GetServiceProviderByIdAsync(Guid id);
    Task<ServiceProviderOutDto> UpdateServiceProviderAsync(Guid id, ServiceProviderInputDto providerInput);
    Task<List<ProviderSummaryOutDto>> ListServiceProvidersAsync(int? limit);
    
    // Apenas para testes:
    Task DeleteServiceProviderAsync(Guid id);
}
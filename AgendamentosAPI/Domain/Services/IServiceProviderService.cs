using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Domain.Services;

public interface IServiceProviderService
{
    Task <ServiceProviderOutDto>  AddServiceProviderAsync(ServiceProviderInputDto providerInput);
    Task<ServiceProviderOutDto> GetServiceProviderByIdAsync(Guid id);
    Task<ServiceProviderOutDto> UpdateServiceProviderAsync(Guid id, ServiceProviderInputDto providerInput);
    Task<List<ProviderSummaryOutDto>> ListServiceProvidersAsync(int? limit);
    
    // Apenas para testes:
    Task DeleteServiceProviderAsync(Guid id);
}
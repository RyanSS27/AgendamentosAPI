using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Domain.Ports;

public interface IServiceProviderService
{
    Task <ServiceProviderOutDto>  AddServiceProviderAsync(ServiceProviderInputDto serviceProviderInput);
    Task<ServiceProviderOutDto> GetServiceProviderByIdAsync(Guid id);
    Task<ServiceProviderOutDto> UpdateServiceProviderAsync(Guid id, ServiceProviderInputDto serviceProviderInput);
    Task<List<ServiceProviderOutDto>> ListServiceProvidersAsync();
    
    // Apenas para testes:
    Task DeleteServiceProviderAsync(Guid id);
}
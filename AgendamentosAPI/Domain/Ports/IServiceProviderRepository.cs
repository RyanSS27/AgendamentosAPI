using AgendamentosAPI.Dtos;
using ServiceProvider = AgendamentosAPI.Domain.Entities.ServiceProvider;
namespace AgendamentosAPI.Domain.Ports;

public interface IServiceProviderRepository
{
    Task AddServiceProviderAsync(ServiceProvider serviceProvider);
    Task<ServiceProvider?> GetServiceProviderByIdAsync(Guid id);
    Task UpdateServiceProviderAsync(ServiceProvider serviceProvider);
    Task<List<ServiceProviderOutDto>> ListServiceProvidersAsync();
    
    // Apenas para testes:
    Task DeleteServiceProviderAsync(ServiceProvider serviceProvider);
}
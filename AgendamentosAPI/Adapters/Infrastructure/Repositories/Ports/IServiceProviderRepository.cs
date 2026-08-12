using AgendamentosAPI.Dtos;
using ServiceProvider = AgendamentosAPI.Domain.Entities.ServiceProvider;
namespace AgendamentosAPI.Adapters.Infrastructure.Repositories.Ports;

public interface IServiceProviderRepository
{
    Task AddServiceProviderAsync(ServiceProvider serviceProvider);
    Task<ServiceProvider?> GetServiceProviderByIdAsync(Guid id);
    Task UpdateServiceProviderAsync(ServiceProvider serviceProvider);
    Task<List<ProviderSummaryOutDto>> ListServiceProvidersAsync(int limit);
    
    // Apenas para testes:
    Task DeleteServiceProviderAsync(ServiceProvider serviceProvider);
}
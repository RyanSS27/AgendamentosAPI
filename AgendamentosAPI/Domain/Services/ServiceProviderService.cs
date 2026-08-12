using AgendamentosAPI.Adapters.Infrastructure.Repositories.Ports;
using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Dtos;
using ServiceProvider = AgendamentosAPI.Domain.Entities.ServiceProvider;

namespace AgendamentosAPI.Domain.Services;

public class ServiceProviderService(IServiceProviderRepository repository) : IServiceProviderService
{
    public readonly int LimitPerRequest = 25;
    
    public async Task<ServiceProviderOutDto> AddServiceProviderAsync(ServiceProviderInputDto input)
    {
        var provider = new ServiceProvider(
            input.Name,
            input.Email
        );
        
        await repository.AddServiceProviderAsync(provider);
        return MapToOutDto(provider);
    }

    public async Task<ServiceProviderOutDto> GetServiceProviderByIdAsync(Guid id)
    {
        var provider = await repository.GetServiceProviderByIdAsync(id);

        if (provider is null)
            throw new NotFoundException($"Prestador de serviço de id '{id}' não encontrado.");

        return MapToOutDto(provider);
    }

    public async Task<ServiceProviderOutDto> UpdateServiceProviderAsync(Guid id, ServiceProviderInputDto input)
    {
        var provider = await repository.GetServiceProviderByIdAsync(id);

        if (provider is null)
            throw new NotFoundException($"Prestador de serviço de id '{id}' não encontrado.");

        provider.UpdateDetails(input.Name, input.Email); 
        
        await repository.UpdateServiceProviderAsync(provider);

        return MapToOutDto(provider);
    }

    public async Task<List<ProviderSummaryOutDto>> ListServiceProvidersAsync(int? limit)
    {
        if (limit is null || limit <= 0)
            return [];

        if (limit > 25)
            limit = LimitPerRequest;

        return await repository.ListServiceProvidersAsync(limit.Value);
    }

    public async Task DeleteServiceProviderAsync(Guid id)
    {
        var provider = await repository.GetServiceProviderByIdAsync(id);

        if (provider is null)
            throw new NotFoundException($"Prestador de serviço de id '{id}' não encontrado.");

        await repository.DeleteServiceProviderAsync(provider);
    }

    private static ServiceProviderOutDto MapToOutDto(ServiceProvider provider)
    {
        return new ServiceProviderOutDto(
            provider.Id, 
            provider.Name, 
            provider.Email, 
            provider.IsActive
        );
    }
}
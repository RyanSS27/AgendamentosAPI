using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos;
using ServiceProvider = AgendamentosAPI.Domain.Entities.ServiceProvider;

namespace AgendamentosAPI.Domain.Services;

public class ServiceProviderService(IServiceProviderRepository serviceProviderRepository) : IServiceProviderService 
{
    public async Task<ServiceProviderOutDto> AddServiceProviderAsync(ServiceProviderInputDto input)
    {
        var provider = new ServiceProvider(
            input.Name,
            input.Email
        );
        
        await serviceProviderRepository.AddServiceProviderAsync(provider);
        return MapToOutDto(provider);
    }

    public async Task<ServiceProviderOutDto> GetServiceProviderByIdAsync(Guid id)
    {
        var provider = await serviceProviderRepository.GetServiceProviderByIdAsync(id);

        if (provider is null)
            throw new NotFoundException($"Prestador de serviço de id '{id}' não encontrado.");

        return MapToOutDto(provider);
    }

    public async Task<ServiceProviderOutDto> UpdateServiceProviderAsync(Guid id, ServiceProviderInputDto input)
    {
        var provider = await serviceProviderRepository.GetServiceProviderByIdAsync(id);

        if (provider is null)
            throw new NotFoundException($"Prestador de serviço de id '{id}' não encontrado.");

        provider.UpdateDetails(input.Name, input.Email); 
        
        await serviceProviderRepository.UpdateServiceProviderAsync(provider);

        return MapToOutDto(provider);
    }

    public Task<List<ServiceProviderOutDto>> ListServiceProvidersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteServiceProviderAsync(Guid id)
    {
        var provider = await serviceProviderRepository.GetServiceProviderByIdAsync(id);

        if (provider is null)
            throw new NotFoundException($"Prestador de serviço de id '{id}' não encontrado.");

        await serviceProviderRepository.DeleteServiceProviderAsync(provider);
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
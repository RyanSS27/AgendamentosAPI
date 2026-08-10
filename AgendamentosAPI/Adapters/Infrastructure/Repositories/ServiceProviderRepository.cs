using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos;
using Microsoft.EntityFrameworkCore;
using ServiceProvider = AgendamentosAPI.Domain.Entities.ServiceProvider;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories;

public class ServiceProviderRepository(ApplicationDbContext context) : IServiceProviderRepository
{
    public async Task AddServiceProviderAsync(ServiceProvider serviceProvider)
    {
        await context.ServiceProviders.AddAsync(serviceProvider);
        await context.SaveChangesAsync();
    }

    public async Task<ServiceProvider?> GetServiceProviderByIdAsync(Guid id)
    {
        return  await context.ServiceProviders.FindAsync(id);
    }

    public async Task UpdateServiceProviderAsync(ServiceProvider serviceProvider)
    {
        context.ServiceProviders.Update(serviceProvider);
        await context.SaveChangesAsync();
    }

    public Task<List<ServiceProviderOutDto>> ListServiceProvidersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteServiceProviderAsync(ServiceProvider serviceProvider)
    {
        context.ServiceProviders.Remove(serviceProvider);
        await context.SaveChangesAsync();
    }
}
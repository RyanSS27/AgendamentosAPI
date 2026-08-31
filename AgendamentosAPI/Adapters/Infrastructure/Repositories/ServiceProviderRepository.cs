using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos;
using AgendamentosAPI.Dtos.ServiceProvider;
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

    public async Task<List<ProviderSummaryOutDto>> ListServiceProvidersAsync(int limit)
    {
        return await context.ServiceProviders
            .OrderByDescending(p => p.IsActive)
            .ThenBy(p => p.Name)
            .Select(p => new ProviderSummaryOutDto(p.Id, p.Name, p.IsActive))
            .Take(limit)
            .ToListAsync();
    }

    public async Task DeleteServiceProviderAsync(ServiceProvider serviceProvider)
    {
        context.ServiceProviders.Remove(serviceProvider);
        await context.SaveChangesAsync();
    }
}
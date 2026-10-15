using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Entities.Enums;
using AgendamentosAPI.Domain.Ports.RepositoryPorts;
using AgendamentosAPI.Dtos.Appointments;
using AgendamentosAPI.Dtos.Appointments.Filters;
using Microsoft.EntityFrameworkCore;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories;

public class AppointmentRepository(ApplicationDbContext context) : IAppointmentRepository
{
    public async Task AddAppointmentAsync(Appointment appointment)
    {
        await context.Appointments.AddAsync(appointment);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAppointmentAsync(Appointment appointment)
    {
        context.Appointments.Update(appointment);
        await context.SaveChangesAsync();
    }

    public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
    {
        return await context.Appointments.FindAsync(id);
    }

    public async Task<List<Appointment>> ListAppointmentsByProviderIdAsync(Guid id, int limit)
    {
        return await context.Appointments.
            OrderBy(a => a.CreatedAt)
            .Where(a => a.ProviderId == id)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<PagedResult<Appointment>> SearchAppointmentsAsync(AppointmentFilter filter)
    {
        // Inicia a query sem acionar o banco ainda
        var query = context.Appointments.AsQueryable();

        if (filter.ProviderId.HasValue)
            query = query.Where(a => a.ProviderId == filter.ProviderId.Value);

        if (filter.CustomerId.HasValue)
            query = query.Where(a => a.CustomerId == filter.CustomerId.Value);

        if (filter.Status.HasValue)
            query = query.Where(a => a.Status == filter.Status.Value);

        if (filter.PeriodStart.HasValue)
            query = query.Where(a => a.Start >= filter.PeriodStart.Value);

        if (filter.PeriodEnd.HasValue)
            query = query.Where(a => a.Start <= filter.PeriodEnd.Value);
        
        var totalCount = await query.CountAsync();
        
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize) // tira 1 para começar do zero
            .Take(filter.PageSize)
            .ToListAsync();

        // Aqui ela é traduzida para SQL e executada, com o ToListAsync()
        return new PagedResult<Appointment>(items, totalCount, filter.Page, filter.PageSize);
    }
    
    public async Task<bool> HasConflictAsync(Guid providerId, DateTimeOffset start, DateTimeOffset end)
    {
        return await context.Appointments
            .AnyAsync(a => 
                a.ProviderId == providerId &&
                a.Status == AppointmentStatus.Scheduled && 
                a.Start < end && 
                a.End > start);
    }

    public async Task DeleteAppointmentAsync(Appointment appointment)
    {
        context.Appointments.Remove(appointment);
        await context.SaveChangesAsync();
    }
}
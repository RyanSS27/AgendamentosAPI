using AgendamentosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceProvider = Microsoft.Extensions.DependencyInjection.ServiceProvider;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories;

public class ApplicationDbContext : DbContext
{
    protected ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ServiceProvider> ServiceProviders { get; set; }
}
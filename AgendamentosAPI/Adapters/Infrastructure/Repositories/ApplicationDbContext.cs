using AgendamentosAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ServiceProvider = AgendamentosAPI.Domain.Entities.ServiceProvider;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories;

public class ApplicationDbContext : DbContext
{
    // "options" passa a string de conexão, senha e tudo mais que eu definir no Program 
    // que essa conexão deva ter
    protected ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<ServiceProvider> ServiceProviders { get; set; }
}
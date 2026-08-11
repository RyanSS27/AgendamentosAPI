using AgendamentosAPI.Adapters.Infrastructure.Repositories.Ports;
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AgendamentosAPI.Adapters.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
{
    public async Task AddCustomerAsync(Customer customer)
    {
        await context.Customers.AddAsync(customer);
        await context.SaveChangesAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id)
    {
        return await context.Customers.FindAsync(id);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        context.Customers.Update(customer);
        await context.SaveChangesAsync();
    }

    public async Task<CustomerOutDto> ListCustomersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCustomerAsync(Customer customer)
    {
        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
    }
}
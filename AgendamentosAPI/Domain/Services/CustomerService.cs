using AgendamentosAPI.Adapters.Infrastructure.Repositories;
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Dtos;

namespace AgendamentosAPI.Domain.Services;

public class CustomerService(ICustomerRepository repository) : ICustomerService
{
    public async Task<CustomerOutDto> AddCustomerAsync(CustomerInputDto input)
    {
        var customer = new Customer(
            input.Name,
            input.Cpf,
            input.PhoneNumbers
            );

        await repository.AddCustomerAsync(customer);
        return MapToOutDto(customer);
    }
    // 37129607-411b-4177-8893-d7f27193ff3d

    public async Task<CustomerOutDto> GetCustomerByIdAsync(Guid id)
    {
        var customer = await repository.GetCustomerByIdAsync(id);

        if (customer is null)
            throw new NotFoundException($"Cliente de id '{id}' não encontrado.");
        
        return MapToOutDto(customer);
    }

    public async Task<CustomerOutDto> UpdateCustomerAsync(Guid id, CustomerInputDto input)
    {
        var customer = await repository.GetCustomerByIdAsync(id);

        if (customer is null)
            throw new NotFoundException($"Cliente de id '{id}' não encontrado.");
        
        customer.UpdateDetails(input.Name, input.Cpf, input.PhoneNumbers);
        await repository.UpdateCustomerAsync(customer);
        
        return MapToOutDto(customer);
    }

    public async Task<List<CustomerOutDto>> ListCustomersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var customer = await repository.GetCustomerByIdAsync(id);
        if (customer is null)
            throw new NotFoundException($"Cliente de id '{id}' não encontrado.");

        await repository.DeleteCustomerAsync(customer);
    }
    
    private static CustomerOutDto MapToOutDto(Customer customer)
    {
        return new CustomerOutDto(
            customer.Id,
            customer.Name,
            customer.Cpf,
            customer.PhoneNumbers, 
            customer.IsActive
        );
    }
}
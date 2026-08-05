using System.Text.RegularExpressions;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class ServiceProvider
{
    public Guid Id  { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Email { get; set; }
    
    // Criar os métodos de alterar name
    
    public bool IsActive { get; private set; } = true;

    public ServiceProvider(string name, string? email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("É necessário informar o nome do doutor.");
        
        
        Name = name;
        Email = email;
    }

    public void InactiveAccount()
    {
        IsActive = false;
    }

    public void ActiveAccount()
    {
        IsActive = true;
    }
}
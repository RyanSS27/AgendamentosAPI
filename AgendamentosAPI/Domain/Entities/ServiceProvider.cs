using System.Text.RegularExpressions;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class ServiceProvider
{
    public Guid Id  { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Email { get; private set; }
    
    // Criar os métodos de alterar name
    
    public bool IsActive { get; private set; } = true;

    public ServiceProvider(string name, string? email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("É necessário informar o nome do doutor.");
        
        
        Name = name;
        Email = email;
    }
    
    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("O novo nome não pode ser vazio.");
            
        Name = newName;
    }
    
    public void ChangeEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("O novo Email não pode ser vazio.");
            
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

    public void UpdateDetails(string inputName, string inputEmail)
    {
        ChangeName(inputName);
        ChangeEmail(inputEmail);
    }
}
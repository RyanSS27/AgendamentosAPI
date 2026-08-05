using System.Text.RegularExpressions;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class Doctor
{
    public Guid Id  { get; private set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string? Email { get; set; }
    public string Crm { get; private set; }

    private readonly List<string> _specialities;

    public IReadOnlyCollection<string> Specialities => _specialities.AsReadOnly();
    
    // Criar os métodos de alterar crm e name
    
    public bool IsActive { get; private set; } = true;

    public Doctor(string name, string? email, string crm, List<string> specialties)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("É necessário informar o nome do doutor.");
        
        ValidateCrm(crm);

        if (specialties is null || specialties.Count == 0)
            throw new DomainException("O doutor deve conter ao menos uma especialidade.");
        
        Name = name;
        Email = email;
        Crm = crm.ToUpper();
        _specialities = specialties;
    }

    public void InactiveAccount()
    {
        IsActive = false;
    }

    public void ActiveAccount()
    {
        IsActive = true;
    }

    public void AddSpecialities(string specialty)
    {
        if (string.IsNullOrWhiteSpace(specialty))
            throw new DomainException("A nova especialidade deve conter um nome válido.");
        
        if (!_specialities.Contains(specialty))
            _specialities.Add(specialty);
    }

    public void AlterSpecialities(string oldSpecialty, string newSpecialty)
    {
        if (string.IsNullOrWhiteSpace(oldSpecialty))
            throw new DomainException("O valor da especialidade deve ser válido para comparação.");
        
        if (string.IsNullOrWhiteSpace(newSpecialty))
            throw new DomainException("A nova especialidade deve conter um nome válido.");
        
        oldSpecialty = oldSpecialty.Trim();
        newSpecialty = newSpecialty.Trim();
        
        var index = _specialities.IndexOf(oldSpecialty);
        
        if (index < 0)
            throw new DomainException($"Não há especialidade '{oldSpecialty}' registrada.");

        if (_specialities.IndexOf(newSpecialty) >= 0)
            throw new DomainException($"Especialidade '{newSpecialty}' já registrada.");

        _specialities[index] = newSpecialty;
    } 
    
    public void RemoveSpecialty(string specialty)
    {
        specialty = specialty.Trim();

        if (string.IsNullOrWhiteSpace(specialty))
            throw new DomainException("A especialidade a ser removida deve conter um nome válido.");

        if (!_specialities.Contains(specialty))
            throw new DomainException($"A especialidade '{specialty}' não está registrada para este doutor.");
        
        if (_specialities.Count == 1)
            throw new DomainException("O doutor deve conter ao menos uma especialidade. Não é possível remover a última.");

        _specialities.Remove(specialty);
    }
    
    private static void ValidateCrm(string crm)
    {
        if (string.IsNullOrWhiteSpace(crm))
            throw new DomainException("O CRM é obrigatório.");

        var crmRegex = new Regex(@"^\d{4,10}-[a-zA-Z]{2}$");
        
        if (!crmRegex.IsMatch(crm))
            throw new DomainException("O CRM deve seguir o formato 'NNNNNN-LL', contendo apenas os números e a sigla do estado (ex: 123456-SP).");
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Cpf { get; private set; }
    
    private readonly List<string> _phoneNumbers = [];
    public IReadOnlyCollection<string> PhoneNumbers => _phoneNumbers?.AsReadOnly() ?? [];

    public bool IsActive { get; private set; } = true;

    public Customer(string name, string cpf, List<string>? phoneNumbers)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("É necessário informar o nome do cliente.");
        
        var cleanCpf = ValidateCpf(cpf);
        
        if (phoneNumbers is null || !phoneNumbers.Any())
            throw new DomainException("O cliente deve ter no mínimo 1 número de contato.");
        
        var uniquePhones = phoneNumbers
            .Select(ValidatePhoneNumber)
            .Distinct()
            .ToList();

        Name = name;
        Cpf = cleanCpf;
        _phoneNumbers = uniquePhones;
    }
    
    protected Customer() {}
    
    public void UpdateDetails(string name, string cpf, List<string> phoneNumbers)
    {
        ChangeName(name);

        var cleanCpf = ValidateCpf(cpf);
        if (Cpf != cleanCpf)
            CorrectCpf(cpf);

        if (phoneNumbers is null || !phoneNumbers.Any())
            throw new DomainException("O cliente deve ter no mínimo 1 número de contato.");

        var uniquePhones = phoneNumbers
            .Select(ValidatePhoneNumber)
            .Distinct()
            .ToList();
        
        _phoneNumbers.Clear();
        _phoneNumbers.AddRange(uniquePhones);
    }
    
    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("O novo nome não pode ser vazio.");
            
        Name = newName;
    }

    public void AddPhoneNumber(string phoneNumber)
    {
        var cleanPhone = ValidatePhoneNumber(phoneNumber);
        
        if (!_phoneNumbers.Contains(cleanPhone))
            _phoneNumbers.Add(cleanPhone);
    }

    public void RemovePhoneNumber(string phoneNumber)
    {
        if (_phoneNumbers.Count == 1)
            throw new DomainException("O cliente deve conter ao menos um telefone de contato. Não é possível remover o último.");
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("O número de telefone deve ser válido para remoção.");

        var cleanPhone = Regex.Replace(phoneNumber, @"\D", "");

        if (!_phoneNumbers.Contains(cleanPhone))
            throw new DomainException("Este número não pertence ao cliente.");

        _phoneNumbers.Remove(cleanPhone);
    }
    
    public void ChangePhoneNumber(string oldNumber, string newNumber)
    {
        if (string.IsNullOrWhiteSpace(oldNumber) || string.IsNullOrWhiteSpace(newNumber))
            throw new DomainException("Tanto o número antigo quanto o novo devem ser informados.");

        var cleanOld = Regex.Replace(oldNumber, @"\D", "");
        var cleanNew = ValidatePhoneNumber(newNumber);

        if (!_phoneNumbers.Contains(cleanOld))
            throw new DomainException("O número antigo não pertence a este cliente.");

        if (_phoneNumbers.Contains(cleanNew))
            throw new DomainException("O novo número já está cadastrado para este cliente.");
        
        _phoneNumbers.Remove(cleanOld);
        _phoneNumbers.Add(cleanNew);
    }
    
    public void InactiveAccount()
    {
        IsActive = false;
    }

    public void ActiveAccount()
    {
        IsActive = true;
    }

    private static string ValidateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("O CPF é obrigatório.");

        var cleanCpf = Regex.Replace(cpf, @"\D", "");
        
        if (cleanCpf.Length != 11)
            throw new DomainException("O CPF deve seguir o formato de 11 dígitos numéricos.");

        return cleanCpf;
    }

    private static string ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("O número de telefone não pode ser vazio.");
            
        var cleanPhone = Regex.Replace(phoneNumber, @"\D", "");
        
        if (cleanPhone.Length < 10 || cleanPhone.Length > 11)
             throw new DomainException("O formato do telefone é inválido (deve conter o DDD e o número).");

        return cleanPhone;
    }
    
    public void CorrectCpf(string correctCpf)
    {
        Cpf = ValidateCpf(correctCpf);
    }
}
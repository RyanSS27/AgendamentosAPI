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
        
        ValidateCpf(cpf);
        
        if (phoneNumbers is null || !phoneNumbers.Any())
            throw new DomainException("O cliente deve ter no mínimo 1 número de contato.");
        
        var uniquePhones = phoneNumbers.Distinct().ToList();

        foreach (var phone in uniquePhones)
        {
            ValidatePhoneNumber(phone);
        }

        Name = name;
        Cpf = cpf;
        _phoneNumbers = uniquePhones;
    }
    
    protected Customer() {}
    
    public void UpdateDetails(string name, string cpf, List<string> phoneNumbers)
    {
        ChangeName(name);

        if (Cpf != cpf)
            CorrectCpf(cpf);

        if (phoneNumbers is null || !phoneNumbers.Any())
            throw new DomainException("O cliente deve ter no mínimo 1 número de contato.");

        var uniquePhones = phoneNumbers.Distinct().ToList();
        
        foreach (var phone in uniquePhones)
            ValidatePhoneNumber(phone);
        
        
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
        ValidatePhoneNumber(phoneNumber);
        
        if (!_phoneNumbers.Contains(phoneNumber))
            _phoneNumbers.Add(phoneNumber);
    }

    public void RemovePhoneNumber(string phoneNumber)
    {
        if (_phoneNumbers.Count == 1)
            throw new DomainException("O cliente deve conter ao menos um telefone de contato. Não é possível remover o último.");
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("O número de telefone deve ser válido para remoção.");

        if (!_phoneNumbers.Contains(phoneNumber))
            throw new DomainException("Este número não pertence ao cliente.");


        _phoneNumbers.Remove(phoneNumber);
    }
    
    public void ChangePhoneNumber(string oldNumber, string newNumber)
    {
        if (string.IsNullOrWhiteSpace(oldNumber) || string.IsNullOrWhiteSpace(newNumber))
            throw new DomainException("Tanto o número antigo quanto o novo devem ser informados.");

        if (!_phoneNumbers.Contains(oldNumber))
            throw new DomainException("O número antigo não pertence a este cliente.");

        ValidatePhoneNumber(newNumber);

        if (_phoneNumbers.Contains(newNumber))
            throw new DomainException("O novo número já está cadastrado para este cliente.");
        
        _phoneNumbers.Remove(oldNumber);
        _phoneNumbers.Add(newNumber);
    }
    
    public void InactiveAccount()
    {
        IsActive = false;
    }

    public void ActiveAccount()
    {
        IsActive = true;
    }

    private static void ValidateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("O CPF é obrigatório.");

        // Padrão: 9 números, um hífen e 2 números (xxxxxxxxx-xx).
        var cpfRegex = new Regex(@"^\d{9}-\d{2}$");
        
        if (!cpfRegex.IsMatch(cpf))
            throw new DomainException("O CPF deve seguir o formato: 9 números, um hífen e 2 números (xxxxxxxxx-xx).");
    }

    private static void ValidatePhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new DomainException("O número de telefone não pode ser vazio.");
            
        // Remove caracteres não numéricos comuns para validar se a string tem uma quantidade aceitável de dígitos (ex: 10 ou 11 números no padrão BR).
        var cleanPhone = phoneNumber.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");
        var phoneRegex = new Regex(@"^\d{10,11}$");
        
        if (!phoneRegex.IsMatch(cleanPhone))
             throw new DomainException("O formato do telefone é inválido (deve conter o DDD e o número).");
    }
    
    public void CorrectCpf(string correctCpf)
    {
        ValidateCpf(correctCpf);
        Cpf = correctCpf;
    }
}
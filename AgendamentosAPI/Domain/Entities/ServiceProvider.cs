using System.Text.RegularExpressions;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class ServiceProvider
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string Cpf { get; private set; }
    public string? Email { get; private set; }
    public string? CalendarId { get; private set; }
    
    public TimeOnly WorkStartTime { get; private set; }
    public TimeOnly WorkEndTime { get; private set; }
    
    public bool IsOvernightShift => WorkEndTime < WorkStartTime;

    public bool IsActive { get; private set; } = true;

    public ServiceProvider(
        string name, 
        string cpf,
        string? email, 
        TimeOnly workStartTime, 
        TimeOnly workEndTime, 
        string? calendarId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("É necessário informar o nome do prestador.");
        
        Cpf = ValidateCpf(cpf);
        ValidateWorkSchedule(workStartTime, workEndTime);

        Name = name;
        Email = email;
        WorkStartTime = workStartTime;
        WorkEndTime = workEndTime;
        CalendarId = calendarId;
    }

    protected ServiceProvider() {}
    
    private static string ValidateCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            throw new DomainException("O CPF é obrigatório.");

        var cleanCpf = Regex.Replace(cpf, @"\D", "");
        
        if (cleanCpf.Length != 11)
            throw new DomainException("O CPF deve seguir o formato de 11 dígitos numéricos.");

        return cleanCpf;
    }

    private static void ValidateWorkSchedule(TimeOnly workStartTime, TimeOnly workEndTime)
    {
        if (workStartTime == workEndTime)
            throw new DomainException("O horário de inicio e termino de expediente não podem ser iguais.");
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

    public void ChangeCalendarId(string calendarId)
    {
        if (string.IsNullOrWhiteSpace(calendarId))
            throw new DomainException("O novo Id da agenda não pode ser vazio.");

        CalendarId = calendarId;
    }

    public void InactiveAccount()
    {
        IsActive = false;
    }

    public void ActiveAccount()
    {
        IsActive = true;
    }

    public void UpdateDetails(string name, string cpf, string email, string? calendarId, TimeOnly newStart, TimeOnly newEnd)
    {
        ChangeName(name);
        
        if (Cpf != cpf)
            CorrectCpf(cpf);

        CalendarId = calendarId;
        
        ChangeEmail(email);
        UpdateWorkSchedule(newStart, newEnd);
    }

    private void UpdateWorkSchedule(TimeOnly newStart, TimeOnly newEnd)
    {
        ValidateWorkSchedule(newStart, newEnd);
        WorkStartTime = newStart;
        WorkEndTime = newEnd;
    }
    
    public void CorrectCpf(string correctCpf)
    {
        Cpf = ValidateCpf(correctCpf);
    }
}
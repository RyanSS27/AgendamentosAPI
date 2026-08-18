using AgendamentosAPI.Domain.Entities.Enums;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProviderId { get; private set; }
    public Guid CustomerId { get; private set; }
    
    // Id para integração externa do Google Calendar
    public string? ExternalCalendarId { get; private set; } 
    
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public AppointmentStatus Status { get; private set; } 
    public string? Observations { get; private set; } 

    public Appointment(Guid providerId, Guid customerId, DateTime start, DateTime end, string? observations = null)
    {
        if (providerId == Guid.Empty)
            throw new DomainException("O prestador de serviço é obrigatório.");

        if (customerId == Guid.Empty)
            throw new DomainException("O cliente é obrigatório.");

        ValidateDates(start, end);

        ProviderId = providerId;
        CustomerId = customerId;
        Start = start;
        End = end;
        Observations = observations;
        Status = AppointmentStatus.Scheduled; // Todo agendamento nasce como 'Agendado'
    }

    protected Appointment() {} // Necessário para o EF Core

    private static void ValidateDates(DateTime start, DateTime end)
    {
        // Garante que não é possível criar um agendamento no passado
        if (start < DateTime.UtcNow)
            throw new DomainException("A data de início não pode estar no passado.");

        if (end <= start)
            throw new DomainException("O horário de término deve ser posterior ao horário de início.");
    }

    public void Reschedule(DateTime newStart, DateTime newEnd)
    {
        if (Status == AppointmentStatus.Canceled)
            throw new DomainException("Não é possível remarcar um agendamento cancelado.");

        ValidateDates(newStart, newEnd);

        Start = newStart;
        End = newEnd;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
            throw new DomainException("Não é possível cancelar um agendamento já realizado.");

        Status = AppointmentStatus.Canceled;
    }

    public void Complete()
    {
        if (Status == AppointmentStatus.Canceled)
            throw new DomainException("Não é possível concluir um agendamento cancelado.");

        Status = AppointmentStatus.Completed;
    }

    public void LinkExternalCalendar(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new DomainException("O ID do calendário externo não pode ser vazio.");
            
        ExternalCalendarId = externalId;
    }
}
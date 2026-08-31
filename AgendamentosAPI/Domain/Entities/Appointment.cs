using AgendamentosAPI.Domain.Entities.Enums;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProviderId { get; private set; }
    public Guid CustomerId { get; private set; }
    
    public string? ExternalEventId { get; private set; } 
    
    public DateTimeOffset Start { get; private set; }
    public DateTimeOffset End { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    
    public AppointmentStatus Status { get; private set; } 
    public string? Observations { get; private set; } 
    
    public Appointment(
        Guid providerId,
        Guid customerId,
        DateTimeOffset start, DateTimeOffset end,
        string? observations = null, 
        AppointmentStatus status = AppointmentStatus.Scheduled)
    {
        if (providerId == Guid.Empty)
            throw new DomainException("O prestador de serviço é obrigatório.");

        if (customerId == Guid.Empty)
            throw new DomainException("O cliente é obrigatório.");

        ValidateDatesAndStatus(start, end, status);

        ProviderId = providerId;
        CustomerId = customerId;
        Start = start;
        End = end;
        Observations = observations;
        Status = status; 
    }

    protected Appointment() {}

    private static void ValidateDatesAndStatus(DateTimeOffset start, DateTimeOffset end , AppointmentStatus status)
    {
        if (end <= start)
            throw new DomainException("O horário de término deve ser posterior ao horário de início.");

        bool isPast = start < DateTimeOffset.UtcNow;

        // Se for registrada uma consulta que ocorreu no passado, ela deve constar como Agendada/Cancelado 
        if (isPast && status == AppointmentStatus.Scheduled)
            throw new DomainException("Agendamentos retroativos só podem ser registrados como Concluídos ou Cancelados.");
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

    public void LinkExternalEvent(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new DomainException("O ID do evento externo não pode ser vazio.");
            
        ExternalEventId = externalId;
    }
}
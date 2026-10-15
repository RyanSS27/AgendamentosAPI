using AgendamentosAPI.Domain.Entities.Enums;
using AgendamentosAPI.Domain.Exceptions;

namespace AgendamentosAPI.Domain.Entities;

public class Appointment
{
    private const int MinAppointmentDurationMinutes = 10;
    private const int MaxAppointmentDurationMinutes = 90;

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProviderId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string? ExternalEventId { get; private set; } 
    
    public DateTimeOffset Start { get; private set; }
    public DateTimeOffset End { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } = DateTimeOffset.UtcNow;
    
    public string AppointmentTitle { get; private set; }
    public AppointmentStatus Status { get; private set; } 
    public string? Observations { get; private set; } 
    
    public Appointment(
        Guid providerId,
        Guid customerId,
        DateTimeOffset start, 
        DateTimeOffset end,
        string appointmentTitle,
        AppointmentStatus status = AppointmentStatus.Scheduled,
        string? observations = null)
    {
        if (providerId == Guid.Empty)
            throw new DomainException("O prestador de serviço é obrigatório.");

        if (customerId == Guid.Empty)
            throw new DomainException("O cliente é obrigatório.");
        
        if (string.IsNullOrWhiteSpace(appointmentTitle))
            throw new DomainException("O título do agendamento é obrigatório.");

        ValidateDatesAndStatus(start, end, status);
        
        AppointmentTitle = appointmentTitle;
        ProviderId = providerId;
        CustomerId = customerId;
        Start = start;
        End = end;
        Observations = observations;
        Status = status; 
    }

    protected Appointment() {}

    private static void ValidateDatesAndStatus(DateTimeOffset start, DateTimeOffset end, AppointmentStatus status)
    {
        if (end <= start)
            throw new DomainException("O horário de término deve ser posterior ao horário de início.");

        TimeSpan duration = end - start;
        
        if (duration.TotalMinutes < MinAppointmentDurationMinutes || duration.TotalMinutes > MaxAppointmentDurationMinutes)
            throw new DomainException($"A duração da consulta deve ser de no mínimo {MinAppointmentDurationMinutes} minutos e no máximo {MaxAppointmentDurationMinutes} minutos.");
        
        if (duration.TotalMinutes % 5 != 0)
            throw new DomainException("A duração solicitada deve ser um múltiplo de 5 minutos.");

        bool isPast = start < DateTimeOffset.UtcNow;
        bool isFuture = start > DateTimeOffset.UtcNow;
        
        if (isPast && status == AppointmentStatus.Scheduled)
            throw new DomainException("Agendamentos retroativos só podem ser registrados como Completed (Concluídos) ou Canceled (Cancelados).");
        
        if (isFuture && status == AppointmentStatus.Completed)
            throw new DomainException("Agendamentos futuros não podem ser registrados como 'Completed' (Realizado).");
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
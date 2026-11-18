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
            throw new DomainException("Informe um identificador (ID) válido para o prestador de serviço.");

        if (customerId == Guid.Empty)
            throw new DomainException("Informe um identificador (ID) válido para o cliente.");
        
        if (string.IsNullOrWhiteSpace(appointmentTitle))
            throw new DomainException("Preencha o título do agendamento com um texto descritivo.");

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
            throw new DomainException("Certifique-se de que o horário de término seja posterior ao horário de início.");

        TimeSpan duration = end - start;
        
        if (duration.TotalMinutes < MinAppointmentDurationMinutes || duration.TotalMinutes > MaxAppointmentDurationMinutes)
            throw new DomainException($"Ajuste a duração da consulta para que tenha entre {MinAppointmentDurationMinutes} e {MaxAppointmentDurationMinutes} minutos.");
        
        if (duration.TotalMinutes % 5 != 0)
            throw new DomainException("Ajuste a duração solicitada para que seja um múltiplo exato de 5 minutos (ex: 15, 20, 25).");

        bool isPast = start < DateTimeOffset.UtcNow;
        bool isFuture = start > DateTimeOffset.UtcNow;
        
        if (isPast && status == AppointmentStatus.Scheduled)
            throw new DomainException("Para registrar um agendamento retroativo (no passado), defina o status como 'Completed' (Concluído) ou 'Canceled' (Cancelado).");
        
        if (isFuture && status == AppointmentStatus.Completed)
            throw new DomainException("Para agendamentos futuros, utilize o status 'Scheduled' (Agendado). Não é permitido concluir consultas que ainda não ocorreram.");
    }
    
    public void UpdateDetails(string appointmentTitle, string? observations)
    {
        if (Status == AppointmentStatus.Completed || Status == AppointmentStatus.Canceled)
            throw new DomainException("Apenas agendamentos com status 'Scheduled' podem ser alterados. Crie um novo agendamento caso precise de novas informações.");

        if (string.IsNullOrWhiteSpace(appointmentTitle))
            throw new DomainException("O título do agendamento é necessário.");

        AppointmentTitle = appointmentTitle;
        Observations = observations;
    }

    public void Cancel()
    {
        if (Status == AppointmentStatus.Completed)
            throw new DomainException("Este agendamento já foi realizado e não pode ser cancelado.");

        Status = AppointmentStatus.Canceled;
    }

    public void Complete()
    {
        if (Status == AppointmentStatus.Canceled)
            throw new DomainException("Este agendamento encontra-se cancelado. Para realizar um atendimento, crie um novo agendamento.");

        Status = AppointmentStatus.Completed;
    }

    public void LinkExternalEvent(string externalId)
    {
        if (string.IsNullOrWhiteSpace(externalId))
            throw new DomainException("Forneça um ID de evento externo válido para realizar a sincronização.");
            
        ExternalEventId = externalId;
    }
}
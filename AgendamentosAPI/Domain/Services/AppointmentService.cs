using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Entities.Enums;
using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Domain.Ports.RepositoryPorts;
using AgendamentosAPI.Domain.Ports.ServicePorts;
using AgendamentosAPI.Dtos.Appointments;
using AgendamentosAPI.Dtos.Appointments.Filters;

namespace AgendamentosAPI.Domain.Services;

public class AppointmentService(
    IServiceProviderRepository providerRepository,
    ICustomerRepository customerRepository,
    IAppointmentRepository appointmentRepository,
    ICalendarIntegrationService calendarService) : IAppointmentService
{
    private const int MaxSearchPeriodDays = 35;
    private const int MaxAppointmentDurationMinutes = 90;
    private const int MaxPageSize = 25;
    private const int DefaultPageSize = 15;
    public Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsSlotsAsync(Guid providerId, DateOnly start, DateOnly end)
    {
        throw new NotImplementedException();
    }

    public async Task<FreeSlotsResponse> GetFreePeriodsSlotsAsync(
        Guid providerId,
        DateOnly startDate,
        DateOnly endDate,
        TimeSpan duration)
    {
        if (startDate > endDate)
            throw new DomainException("A data de início não pode ser maior que a data de termino para a busca.");
            
        if (startDate < DateOnly.FromDateTime(DateTime.UtcNow)) 
            throw new DomainException("As datas passadas para busca não podem ser datas já ultrapassadas.");

        if (duration.TotalMinutes < 10 || duration.TotalMinutes > MaxAppointmentDurationMinutes)
            throw new DomainException(
                "A duração da consulta deve ser de no mínimo 10 minutos e no máximo 90 minutos (1:30)");
        
        if (duration.TotalMinutes % 5 != 0)
            throw new DomainException("A duração solicitada deve ser um múltiplo de 5 minutos.");

        var provider = await providerRepository.GetServiceProviderByIdAsync(providerId);
        if (provider is null)
            throw new NotFoundException($"Não há um prestador de serviço registrado com Id '{providerId}'.");

        if (provider.CalendarId is null)
            throw new DomainException("O Id do Calendário do prestador ainda não foi cadastrado.");
        
        if (endDate.DayNumber - startDate.DayNumber > MaxSearchPeriodDays)
        {
            endDate = startDate.AddDays(MaxSearchPeriodDays);
        }
        
        var busyPeriods = await calendarService.GetBusyPeriodsAsync(
            provider.CalendarId, 
            new DateTimeOffset(startDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero), 
            provider.IsOvernightShift ? 
                    // Caso o turno seja noturno, a data de fim do limite recebe +1 dia para cobrir o último turno até o fim
                    new DateTimeOffset(endDate.AddDays(1).ToDateTime(TimeOnly.MaxValue), TimeSpan.Zero)
                    : new DateTimeOffset(endDate.ToDateTime(provider.WorkEndTime), TimeSpan.Zero)); 
        
        var responseDays = new List<DailyFreeTime>(); 
        
        for (var date = startDate; date <= endDate; date = date.AddDays(1)) 
        {
            // Lista secundária que vai compor o nó do JSON "TimePeriods" de cada dia
            var freePeriodsOfDay = new List<TimePeriod>();

            DateTimeOffset startPeriod = new DateTimeOffset(date.ToDateTime(provider.WorkStartTime), TimeSpan.Zero);
            DateTimeOffset shiftEnd = provider.IsOvernightShift ?
                new DateTimeOffset(date.AddDays(1).ToDateTime(provider.WorkEndTime), TimeSpan.Zero)
                : new DateTimeOffset(date.ToDateTime(provider.WorkEndTime), TimeSpan.Zero);

            var busyPeriodsOfTheDate = busyPeriods
                .Where(p => p.Start < shiftEnd && p.End > startPeriod)
                .OrderBy(p => p.Start)
                .ToList();

            if (busyPeriodsOfTheDate.Count == 0)
            {
                freePeriodsOfDay.Add(new TimePeriod(startPeriod, shiftEnd));
            }
            else
            {
                foreach (var busyPeriod in busyPeriodsOfTheDate)
                {
                    if (busyPeriod.Start >= startPeriod)
                    {
                        TimeSpan time = busyPeriod.Start - startPeriod;
                    
                        if (time.TotalMinutes >= duration.TotalMinutes)
                        {
                            freePeriodsOfDay.Add(new TimePeriod(startPeriod, busyPeriod.Start));
                        }
                    }
                    
                    if (busyPeriod.End > startPeriod)
                    {
                        startPeriod = busyPeriod.End;
                    }
                }

                
                // Essa última verificação deve ocorrer, pois o último período não é analisado no laço
                if (startPeriod < shiftEnd)
                {
                    TimeSpan lastGap = shiftEnd - startPeriod;
                    if (lastGap.TotalMinutes >= duration.TotalMinutes) 
                        freePeriodsOfDay.Add(new TimePeriod(startPeriod, shiftEnd));
                }
            }
            
            // Só adiciona o dia à resposta se houver algum horário livre nele
            if (freePeriodsOfDay.Count > 0)
            {
                responseDays.Add(new DailyFreeTime(date, freePeriodsOfDay.AsReadOnly()));
            }
        }

        return new FreeSlotsResponse(responseDays.AsReadOnly());
    }

    public async Task<AppointmentOutDto> CreateEventAsync(string calendarId, AppointmentInputDto appointmentInput)
    {
        var customer = await customerRepository.GetCustomerByIdAsync(appointmentInput.CustomerId);
        if (customer is null)
            throw new NotFoundException($"Não há um cliente registrado com Id '{appointmentInput.CustomerId}'.");

        var provider = await providerRepository.GetServiceProviderByIdAsync(appointmentInput.ProviderId);
        if (provider is null)
            throw new NotFoundException($"Não há um prestador de serviço registrado com Id '{appointmentInput.ProviderId}'.");

        if (string.IsNullOrWhiteSpace(provider.CalendarId))
            throw new DomainException($"O prestador de serviço {provider.Name} ainda não contém agenda cadastrada.");
        
        var appointment = new Appointment(
            appointmentInput.ProviderId,
            appointmentInput.CustomerId,
            appointmentInput.Start,
            appointmentInput.End,
            appointmentInput.Title,
            appointmentInput.Status,
            observations: appointmentInput.Observations
        );
        
        switch (appointment.Status)
        {
            case AppointmentStatus.Scheduled:
                var hasDbConflict = await appointmentRepository.HasConflictAsync(provider.Id, appointment.Start, appointment.End);
                if (hasDbConflict)
                    throw new DomainException("O período informado já possui um agendamento no sistema interno. Consulte os horários livres.");
                
                var ocupacoes = await calendarService.GetBusyPeriodsAsync(provider.CalendarId, appointment.Start, appointment.End);
                if (ocupacoes.Any())
                    throw new DomainException("O período informado está ocupado na agenda externa. Consulte os horários livres.");
                
                var externalId = await calendarService.CreateEventAsync(provider.CalendarId, appointment);
                appointment.LinkExternalEvent(externalId);
                break;

            case AppointmentStatus.Completed:
                var completedExternalId = await calendarService.CreateEventAsync(provider.CalendarId, appointment);
                appointment.LinkExternalEvent(completedExternalId);
                break;

            case AppointmentStatus.Canceled:
                break;
        }
        
        await appointmentRepository.AddAppointmentAsync(appointment);
        
        
        return AppointmentOutDto.FromEntity(appointment);
    }
    

    public Task<AppointmentOutDto> UpdateEventAsync(string calendarId, Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentOutDto> CancelEventAsync(string calendarId, string externalEventId)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<Appointment>> SearchAppointmentAsync(AppointmentFilter filter)
    {
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize > MaxPageSize 
            ? MaxPageSize 
            : (filter.PageSize < 1 ? DefaultPageSize : filter.PageSize);
        if (filter.PageSize > MaxPageSize)
        {
            pageSize = MaxPageSize;
        }
        else if (filter.PageSize < 1 )
        {
            pageSize = DefaultPageSize;
        }
        else
        {
            pageSize = filter.PageSize;
        }
            
        // 2. Cria um novo record higienizado copiando os demais valores (non-destructive mutation)
        var safeFilter = filter with { Page = page, PageSize = pageSize };
        
        return await appointmentRepository.SearchAppointmentsAsync(safeFilter);
    }

    public async Task DeleteAppointmentAsync(Guid appointmentId)
    {
        throw new NotImplementedException();
    }
}
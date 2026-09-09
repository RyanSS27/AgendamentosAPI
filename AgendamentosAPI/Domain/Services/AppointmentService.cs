using System.Data;
using System.Runtime.CompilerServices;
using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Exceptions;
using AgendamentosAPI.Domain.Ports;
using AgendamentosAPI.Domain.Ports.ServicePorts;
using AgendamentosAPI.Dtos.Appointment;

namespace AgendamentosAPI.Domain.Services;

public class AppointmentService(
    IServiceProviderRepository providerRepository,
    ICustomerRepository customerRepository,
    ICalendarIntegrationService calendarService) : IAppointmentService
{
    private const int MaxSearchPeriodDays = 35;
    private const int MaxAppointmentDurationMinutes = 90;
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
        
        // A duração deve ser obrigatoriamente um múltiplo de 5 minutos para alinhamento da grade de horários
        if (duration.TotalMinutes % 5 != 0)
            throw new DomainException("A duração solicitada deve ser um múltiplo de 5 minutos.");

        var provider = await providerRepository.GetServiceProviderByIdAsync(providerId);
        if (provider is null)
            throw new NotFoundException("Prestador de serviço não encontrado para o id '" + providerId + "'.");

        if (provider.CalendarId is null)
            throw new DomainException("O Id do Calendário do prestador ainda não foi cadastrado.");
        
        // Se, por acaso o número de dias para a pesquisa exceder a variável de controle, considere o período máximo de busca
        if (endDate.DayNumber - startDate.DayNumber > MaxSearchPeriodDays)
        {
            endDate = startDate.AddDays(MaxSearchPeriodDays);
        }
        
        var busyPeriods = await calendarService.GetBusyPeriodsAsync(
            provider.CalendarId, 
            new DateTimeOffset(startDate.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero), 
            provider.IsOvernightShift ? 
                    // Caso o turno seja noturno, a data de fim do limite recebe +1 dia para cobrir o último turno
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

    public Task<AppointmentOutDto> CreateEventAsync(string calendarId, Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentOutDto> UpdateEventAsync(string calendarId, Appointment appointment)
    {
        throw new NotImplementedException();
    }

    public Task<AppointmentOutDto> CancelEventAsync(string calendarId, string externalEventId)
    {
        throw new NotImplementedException();
    }
}
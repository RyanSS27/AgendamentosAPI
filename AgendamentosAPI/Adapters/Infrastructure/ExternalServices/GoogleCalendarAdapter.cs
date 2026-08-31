using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Domain.Ports;
using Flurl.Http;

namespace AgendamentosAPI.Adapters.Infrastructure.ExternalServices;

public class GoogleCalendarAdapter : ICalendarIntegrationPort
{
    private const string BaseUrl = "https://www.googleapis.com/calendar/v3";
    private readonly IGoogleTokenProvider _tokenProvider;

    // Injetamos um provedor de token para isolar a complexidade de gerar o JWT da Service Account
    public GoogleCalendarAdapter(IGoogleTokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public async Task<IReadOnlyCollection<TimePeriod>> GetBusyPeriodsAsync(string calendarId, DateTimeOffset start, DateTimeOffset end)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        // O endpoint FreeBusy do Google exige um POST com timeMin e timeMax em formato RFC 3339
        var requestBody = new
        {
            timeMin = start.ToString("O"), 
            timeMax = end.ToString("O"),
            items = new[] { new { id = calendarId } }
        };

        var response = await $"{BaseUrl}/freeBusy"
            .WithOAuthBearerToken(token)
            .PostJsonAsync(requestBody)
            .ReceiveJson<GoogleFreeBusyResponse>(); 

        var busyPeriods = new List<TimePeriod>();
        
        if (response.Calendars.TryGetValue(calendarId, out var calendarInfo))
        {
            foreach (var busy in calendarInfo.Busy)
            {
                busyPeriods.Add(new TimePeriod(busy.Start, busy.End));
            }
        }

        return busyPeriods;
    }

    public async Task<string> CreateEventAsync(string calendarId, Appointment appointment)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        var requestBody = new
        {
            summary = "Consulta Agendada",
            description = appointment.Observations,
            start = new { dateTime = appointment.Start.ToString("O") },
            end = new { dateTime = appointment.End.ToString("O") }
        };

        var response = await $"{BaseUrl}/calendars/{calendarId}/events"
            .WithOAuthBearerToken(token)
            .PostJsonAsync(requestBody)
            .ReceiveJson<GoogleEventResponse>();

        return response.Id; // Este é o ExternalEventId que será salvo no nosso banco
    }

    public async Task UpdateEventAsync(string calendarId, Appointment appointment)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        var requestBody = new
        {
            description = appointment.Observations,
            start = new { dateTime = appointment.Start.ToString("O") },
            end = new { dateTime = appointment.End.ToString("O") }
        };

        await $"{BaseUrl}/calendars/{calendarId}/events/{appointment.ExternalEventId}"
            .WithOAuthBearerToken(token)
            .PatchJsonAsync(requestBody)
            .ReceiveString();
    }

    public async Task CancelEventAsync(string calendarId, string externalEventId)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        await $"{BaseUrl}/calendars/{calendarId}/events/{externalEventId}"
            .WithOAuthBearerToken(token)
            .DeleteAsync();
    }
}

// DTOs internos (privados a esta camada de infraestrutura) para mapear o JSON do Google
internal class GoogleFreeBusyResponse
{
    public Dictionary<string, CalendarBusyInfo> Calendars { get; set; } = new();
}

internal class CalendarBusyInfo
{
    public List<BusyPeriod> Busy { get; set; } = new();
}

internal class BusyPeriod
{
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
}

internal class GoogleEventResponse
{
    public string Id { get; set; } = string.Empty;
}
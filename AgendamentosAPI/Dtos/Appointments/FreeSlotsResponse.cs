namespace AgendamentosAPI.Dtos.Appointments;

public record FreeSlotsResponse(IReadOnlyCollection<DailyFreeTime> Days);
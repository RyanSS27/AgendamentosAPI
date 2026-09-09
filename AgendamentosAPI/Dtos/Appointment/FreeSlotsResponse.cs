namespace AgendamentosAPI.Dtos.Appointment;

public record FreeSlotsResponse(IReadOnlyCollection<DailyFreeTime> Days);
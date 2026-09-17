using AgendamentosAPI.Domain.Ports.ServicePorts;

namespace AgendamentosAPI.Dtos.Appointment;

public record DailyFreeTime(DateOnly Date, IReadOnlyCollection<TimePeriod> TimePeriods);
using AgendamentosAPI.Domain.Ports.ServicePorts;

namespace AgendamentosAPI.Dtos.Appointments;

public record DailyFreeTime(DateOnly Date, IReadOnlyCollection<TimePeriod> TimePeriods);
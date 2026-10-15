using AgendamentosAPI.Domain.Entities;
using AgendamentosAPI.Dtos.Appointments;
using AgendamentosAPI.Dtos.Appointments.Filters;

namespace AgendamentosAPI.Domain.Ports.RepositoryPorts;

public interface IAppointmentRepository
{
    Task AddAppointmentAsync(Appointment appointment);
    Task UpdateAppointmentAsync(Appointment appointment);
    Task<Appointment?> GetAppointmentByIdAsync(Guid id);
    Task<PagedResult<Appointment>> SearchAppointmentsAsync(AppointmentFilter filter);
    Task<bool> HasConflictAsync(Guid providerId, DateTimeOffset start, DateTimeOffset end);

    Task DeleteAppointmentAsync(Appointment appointment);
}
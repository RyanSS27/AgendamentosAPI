namespace AgendamentosAPI.Domain.Ports.ServicePorts;

public interface IGoogleTokenProvider
{
    Task<string> GetAccessTokenAsync();
}
namespace AgendamentosAPI.Domain.Ports;

public interface IGoogleTokenProvider
{
    Task<string> GetAccessTokenAsync();
}
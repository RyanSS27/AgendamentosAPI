using AgendamentosAPI.Domain.Ports;
using Google.Apis.Auth.OAuth2;

namespace AgendamentosAPI.Adapters.Infrastructure.ExternalServices;

public class GoogleServiceAccountTokenProvider : IGoogleTokenProvider
{
    private readonly IConfiguration _configuration;

    public GoogleServiceAccountTokenProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GetAccessTokenAsync()
    {
        // 1. Busca o caminho do arquivo configurado no appsettings
        var credentialPath = _configuration["GoogleCalendar:CredentialFilePath"];

        if (string.IsNullOrWhiteSpace(credentialPath) || !File.Exists(credentialPath))
            throw new InvalidOperationException("O arquivo de credenciais do Google não foi encontrado no caminho especificado.");

        // 2. Lê o arquivo JSON com segurança
        await using var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read);
        
        // 3. Cria a credencial solicitando o escopo específico do Calendar
        var credential = GoogleCredential.FromStream(stream)
            .CreateScoped("https://www.googleapis.com/auth/calendar");

        // 4. Gera/Recupera o token JWT (Bearer)
        var token = await credential.UnderlyingCredential.GetAccessTokenForRequestAsync();
        
        return token;
    }
}
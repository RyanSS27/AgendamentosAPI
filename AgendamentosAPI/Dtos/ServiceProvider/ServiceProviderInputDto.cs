using System.ComponentModel.DataAnnotations;

namespace AgendamentosAPI.Dtos.ServiceProvider;

public record ServiceProviderInputDto(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    string Name,

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    string Email,
    
    string? CalendarId,
    
    TimeOnly WorkStartTime,
    TimeOnly WorkEndTime
    );
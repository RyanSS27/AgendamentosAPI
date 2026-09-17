using System.ComponentModel.DataAnnotations;

namespace AgendamentosAPI.Dtos.ServiceProvider;

public record ServiceProviderInputDto(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    string Name,

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve conter 11 dígitos numéricos ou o formato 000.000.000-00.")]
    string Cpf,

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    string Email,

    string? CalendarId,

    [Required(ErrorMessage = "O horário de início do expediente é obrigatório.")]
    TimeOnly WorkStartTime,

    [Required(ErrorMessage = "O horário de término do expediente é obrigatório.")]
    TimeOnly WorkEndTime
);
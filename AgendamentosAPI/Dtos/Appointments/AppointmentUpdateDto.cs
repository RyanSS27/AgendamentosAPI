using System.ComponentModel.DataAnnotations;

namespace AgendamentosAPI.Dtos.Appointments;

public record AppointmentUpdateDto(
    [Required(ErrorMessage = "Forneça um título descritivo para o agendamento (ex: 'Consulta de Rotina').")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve conter entre 3 e 100 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9\sÀ-ÿ,.\-'\(\)]+$", ErrorMessage = "Utilize apenas letras, números, espaços e pontuações comuns no título.")]
    string Title,

    [StringLength(500, ErrorMessage = "O texto das observações excedeu o limite máximo de 500 caracteres. Por favor, resuma as informações.")]
    [RegularExpression(@"^[a-zA-Z0-9\sÀ-ÿ,.\-'\(\)\n\r]*$", ErrorMessage = "Utilize apenas texto padrão nas observações, sem caracteres especiais ou códigos HTML/JS.")]
    string? Observations = null
);
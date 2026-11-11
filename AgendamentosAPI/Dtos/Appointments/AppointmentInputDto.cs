using System.ComponentModel.DataAnnotations;
using AgendamentosAPI.Domain.Entities.Enums;

namespace AgendamentosAPI.Dtos.Appointments;

public record AppointmentInputDto(
    [Required(ErrorMessage = "O Id do prestador de serviço é necessário.")]
    Guid ProviderId,

    [Required(ErrorMessage = "O Id do cliente é necessário.")]
    Guid CustomerId,

    [Required(ErrorMessage = "Informe a data e o horário de início do agendamento.")]
    DateTimeOffset Start,

    [Required(ErrorMessage = "Informe a data e o horário de término do agendamento.")]
    DateTimeOffset End,

    [Required(ErrorMessage = "Forneça um título descritivo para o agendamento (ex: 'Consulta de Rotina').")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O título deve conter entre 3 e 100 caracteres.")]
    [RegularExpression(@"^[a-zA-Z0-9\sÀ-ÿ,.\-'\(\)]+$", ErrorMessage = "Utilize apenas letras, números, espaços e pontuações comuns no título.")]
    string Title,

    [EnumDataType(typeof(AppointmentStatus), ErrorMessage = "Selecione um status válido (ex: 0 para Agendado, 1 para Concluído, 2 para Cancelado).")]
    AppointmentStatus Status = AppointmentStatus.Scheduled,

    [StringLength(500, ErrorMessage = "O texto das observações excedeu o limite máximo de 500 caracteres. Por favor, resuma as informações.")]
    [RegularExpression(@"^[a-zA-Z0-9\sÀ-ÿ,.\-'\(\)\n\r]*$", ErrorMessage = "Utilize apenas texto padrão nas observações, sem caracteres especiais ou códigos HTML/JS.")]
    string? Observations = null
);
using System.ComponentModel.DataAnnotations;

namespace AgendamentosAPI.Dtos.Customer;

public record CustomerInputDto(
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres.")]
    string Name,

    [Required(ErrorMessage = "O CPF é obrigatório.")]
    [RegularExpression(@"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "O CPF deve conter 11 dígitos numéricos ou o formato 000.000.000-00.")]
    string Cpf,

    [Required(ErrorMessage = "A lista de telefones é obrigatória.")]
    [MinLength(1, ErrorMessage = "O cliente deve ter no mínimo 1 número de contato.")]
    List<string> PhoneNumbers 
);
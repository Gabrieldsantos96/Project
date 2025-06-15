using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Dtos.User;
public sealed class SigninInput
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Email inválido")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Senha é obrigatória")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
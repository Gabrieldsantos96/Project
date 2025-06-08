using Project.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Dtos.User;
public sealed class ResetPasswordInput
{
    [Required(ErrorMessage = ValidationMessages.FieldRequired)]
    [MinLength(8, ErrorMessage = "A senha deve conter pelo menos 6 caracteres")]
    [MaxLength(128, ErrorMessage = "A senha deve possuir no máximo 128 caracteres")]
    [PasswordPolicy(ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um símbolo.")]
    public string Password { get; set; } = null!;


    [Required(ErrorMessage = ValidationMessages.FieldRequired)]
    [Compare(nameof(Password), ErrorMessage = "As senhas não conferem")]
    public string ConfirmPassword { get; set; } = null!;

    [Required]
    public string? EncodedToken { get; set; } = null!;

    [Required]
    public string? EncodedEmail { get; set; } = null!;
}
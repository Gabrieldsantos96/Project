using Project.Shared.Validations;
using System.ComponentModel.DataAnnotations;


namespace Project.Shared.Dtos.User;
public sealed class ChangePasswordInput
{
    [Required(ErrorMessage = ValidationMessages.FieldRequired)]
    public string CurrentPassword { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FieldRequired)]
    [MinLength(8, ErrorMessage = "A nova senha deve ter no mínimo 8 caracteres, incluindo letra maiúscula, letra minúscula, número e símbolo especial.")]
    [MaxLength(128, ErrorMessage = "A senha deve possuir no máximo 128 caracteres")]
    [PasswordPolicy(ErrorMessage = "A nova senha deve ter no mínimo 8 caracteres, incluindo letra maiúscula, letra minúscula, número e símbolo especial.")]

    public string NewPassword { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FieldRequired)]
    [Compare(nameof(NewPassword), ErrorMessage = "As senhas não conferem")]
    public string ConfirmPassword { get; set; } = null!;
}
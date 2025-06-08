using Project.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Dtos.User;
public class ResetPasswordRequestInput
{
    [Required(ErrorMessage = ValidationMessages.FieldRequired)]
    public required string Email { get; set; }
};

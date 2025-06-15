using System.ComponentModel.DataAnnotations;

namespace Project.Shared.Dtos.User;
public sealed class RefreshTokenInput
{
    [Required]
    public string RefreshToken { get; set; } = null!;
}

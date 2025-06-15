using Project.Domain.Interfaces.Infra;
using Project.Shared.Dtos.User;
using Project.Shared.Validations;
using AppAny.HotChocolate.FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Project.Domain.Entities;

namespace Project.Application.EntryPoints.User.Mutations;
public sealed class CreateAccountInputValidator : AbstractValidator<CreateAccountInput>
{
    public CreateAccountInputValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage(ValidationMessages.FieldRequired);

        RuleFor(s => s.Email)
            .NotEmpty().WithMessage(ValidationMessages.FieldRequired);

        RuleFor(s => s.Password)
            .NotEmpty().WithMessage(ValidationMessages.FieldRequired);
    }
}
public interface ICreateAccountResolver
{
    Task<UserProfileDto> CreateAccountAsync(CreateAccountInput input, CancellationToken ct);
}
public sealed class CreateAccountResolver(UserManager<ProjectUser> userManager) : ICreateAccountResolver
{
    public async Task<UserProfileDto> CreateAccountAsync([UseFluentValidation, UseValidator<CreateAccountInputValidator>] CreateAccountInput input, CancellationToken ct)
    {
        var user = new ProjectUser
        {
            Name = input.Name,
            Email = input.Email,
            UserName = input.Email
        };

        var result = await userManager.CreateAsync(user, input.Password);

        if (!result.Succeeded)
        {
            throw new ArgumentException(string.Join("; ", result.Errors.Select(e => e.Description)));
        }

        return new UserProfileDto()
        {
            Id = user.RefId,
            UserName = user.Name,
            Email = user.Email
        };
    }
}
using Project.Application.Features.User.Queries;
using Project.Shared.Dtos.User;

namespace Project.Application.Graphql;
public class Queries()
{
    public Task<UserProfileDto> GetUserProfileAsync([Service] IGetUserProfileResolver _getUserProfileResolver, CancellationToken ct) => _getUserProfileResolver.GetUserProfileAsync(ct);
}

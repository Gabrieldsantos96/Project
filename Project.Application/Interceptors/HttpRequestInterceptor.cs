using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Project.Application.Services;

namespace Project.Application.Interceptors;
public sealed class HttpRequestInterceptor(ILogger<HttpRequestInterceptor> logger, IServiceScopeFactory serviceScopeFactory) : DefaultHttpRequestInterceptor
{
    public override async ValueTask OnCreateAsync(
        HttpContext context,
        IRequestExecutor requestExecutor,
        OperationRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var authorization) &&
            authorization.FirstOrDefault() is string header &&
            header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var jwt = header["Bearer ".Length..].Trim();

            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var tokenService = scope.ServiceProvider.GetRequiredService<ITokenValidator>();
                var claimsPrincipal = tokenService.ValidateJwt(jwt);

                if (claimsPrincipal.Identity != null && claimsPrincipal.Identity.IsAuthenticated)
                {
                    context.User = claimsPrincipal;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (SecurityTokenException ex)
            {
                logger.LogError(ex, "Erro ao validar JWT.");
            }
        }
        else
        {
            logger.LogWarning("");
        }

        await base.OnCreateAsync(context, requestExecutor, requestBuilder, cancellationToken);
    }
}

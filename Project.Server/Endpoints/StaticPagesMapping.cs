using Microsoft.AspNetCore.Mvc;
using Project.Server.Templates;

namespace Project.Server.Endpoints;

public static class StaticPagesMapping
{
    public static void MapStaticPages(this IEndpointRouteBuilder app)
    {
        app.MapGet("/StatusCode/{404}", () => Results.Content(HostedTemplates.NotFoundPage, "text/html")).WithMetadata(new ApiExplorerSettingsAttribute { IgnoreApi = true });
    }
}
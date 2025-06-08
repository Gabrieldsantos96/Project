using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Project.Application.Services;
public class RazorTemplateRenderer(HtmlRenderer htmlRenderer)
{
    public async Task<string> RenderAsync<T>(object model) where T : IComponent
    {
        var html = await htmlRenderer.Dispatcher.InvokeAsync(async () =>
        {
            var dictionary = new Dictionary<string, object?>
            {
                { "Model", model }
            };

            var parameters = ParameterView.FromDictionary(dictionary);

            var output = await htmlRenderer.RenderComponentAsync<T>(parameters);

            return output.ToHtmlString();
        });

        return html;
    }
}
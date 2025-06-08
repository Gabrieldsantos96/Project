using Project.Application.DI;
using Project.Infra.DI;
using Project.Shared.Consts;
using Project.Server.Endpoints;
using Project.Server.Utils;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Project.Server.DI;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new String("Azure:KeyVault:Uri");

//var keyVaultEndpoint = new Uri(builder.Configuration["Azure:KeyVault:Uri"]!);

//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = FileUploadConsts.MaxVideoFileSize;
});


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization(options => options.SerializeAllClaims = true);

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionToProblemDetailsHandler>();

builder.Services.AddSignalR();

if (builder.Environment.IsProduction())
{
    builder.Services.AddResponseCompression(options =>
    {
        options.Providers.Add<BrotliCompressionProvider>();
        options.Providers.Add<GzipCompressionProvider>();

        options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat([
            "text/plain",
            "text/css",
            "application/javascript",
            "text/html",
            "application/xml",
            "text/xml",
            "application/json",
            "image/svg+xml",
            "application/octet-stream"
        ]);
    });
}

builder.Services.AddWebServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseStatusCodePages();

app.UseExceptionHandler();

if (builder.Environment.IsProduction())
{
    app.UseResponseCompression();
}

app.UseRequestLocalization("pt-BR");

app.UseRequestLocalization(new RequestLocalizationOptions()
    .AddSupportedCultures("pt-BR")
    .AddSupportedUICultures("pt-BR"));

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapGraphQL("/graphql");

app.UseAntiforgery();

app.MapStaticPages();

await Seeding.SeedDatabaseAsync(app.Services, builder.Environment);

app.Run();

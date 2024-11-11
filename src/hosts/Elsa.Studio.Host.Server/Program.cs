using Elsa.Studio.Core.BlazorServer.Extensions;
using Elsa.Studio.Dashboard.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Login.BlazorServer.Extensions;
using Elsa.Studio.Login.HttpMessageHandlers;
using Elsa.Studio.Models;
using Elsa.Studio.Secrets;
using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Webhooks.Extensions;
using Elsa.Studio.WorkflowContexts.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Workflows.Designer.Extensions;
using MudBlazor.Translations;
using MudBlazor.Services;
using Elsa.Studio.Resources;

// Build the host.
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
CultuerOptions appOptions = builder.Configuration.GetSection("AppOptions").Get<CultuerOptions>()!;
builder.Services.Configure<CultuerOptions>(builder.Configuration.GetSection("AppOptions"));

// Register Razor services.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options =>
{
    // Register the root components.
    options.RootComponents.RegisterCustomElsaStudioElements();
    options.RootComponents.MaxJSRootComponents = 1000;
});

// Register shell services and modules.
var backendApiConfig = new BackendApiConfig
{
    ConfigureBackendOptions = options => configuration.GetSection("Backend").Bind(options),
    ConfigureHttpClientBuilder = options => options.AuthenticationHandler = typeof(AuthenticatingApiHttpMessageHandler), 
};

builder.Services.AddCore();
//builder.Services.AddShell(options => configuration.GetSection("Shell").Bind(options));
builder.Services.AddShell(options => options.DisableAuthorization = true);

builder.Services.AddRemoteBackend(backendApiConfig);
builder.Services.AddLoginModule();
builder.Services.AddDashboardModule();
builder.Services.AddWorkflowsModule();
builder.Services.AddWorkflowContextsModule();
builder.Services.AddWebhooksModule();
builder.Services.AddAgentsModule(backendApiConfig);
builder.Services.AddSecretsModule(backendApiConfig);
//builder.Services.AddLocalizationInterceptor<MudTranslationsInterceptor>();
//builder.Services.AddMudTranslations();
builder.Services.AddLocalization();

// Configure SignalR.
builder.Services.AddSignalR(options =>
{
    // Set MaximumReceiveMessageSize:
    options.MaximumReceiveMessageSize = 5 * 1024 * 1000; // 5MB
});

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseRequestLocalization(
new RequestLocalizationOptions()
        .SetDefaultCulture(appOptions.SupportedCultures[0])
        .AddSupportedCultures(appOptions.SupportedCultures)
        .AddSupportedUICultures(appOptions.SupportedCultures)
);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Run the application.
app.Run();
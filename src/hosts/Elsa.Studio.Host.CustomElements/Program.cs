using Elsa.Studio.Shell.Extensions;
using Elsa.Studio.Workflows.Extensions;
using Elsa.Studio.Contracts;
using Elsa.Studio.Core.BlazorWasm.Extensions;
using Elsa.Studio.Extensions;
using Elsa.Studio.Host.CustomElements.Components;
using Elsa.Studio.Workflows.Designer.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

// Build the host.
var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;

// Register the custom elements.
builder.RootComponents.RegisterCustomElsaStudioElements();
builder.RootComponents.RegisterCustomElement<WorkflowDefinitionEditorWrapper>("elsa-studio-workflow-definition-editor");

// Register the modules.
builder.Services.AddCore();
builder.Services.AddShell();
builder.Services.AddRemoteBackend(options => configuration.GetSection("Backend").Bind(options));
builder.Services.AddWorkflowsModule();

// Build the application.
var app = builder.Build();

// Run each startup task.
var startupTask = app.Services.GetServices<IStartupTask>();
foreach (var task in startupTask) await task.ExecuteAsync();

// Run the application.
await app.RunAsync();
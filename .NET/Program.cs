using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TogglMCP.Services;
using TogglMCP.Tools;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services.AddHttpClient<TogglApiService>(client =>
{
    client.DefaultRequestHeaders.Add("User-Agent", "Toggl MCP Server 1.0");
    client.Timeout = TimeSpan.FromSeconds(30);
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<TogglTools>();

builder.Services.AddScoped<TogglTools>();

var host = builder.Build();

try
{
    await host.RunAsync();
}
catch (OperationCanceledException)
{
}

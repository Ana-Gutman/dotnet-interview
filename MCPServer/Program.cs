using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using System.Net.Http.Headers;
using ModelContextProtocol.Server;

using McpServer.Tools;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

builder.Services.AddSingleton(_ =>
{
    var client = new HttpClient { BaseAddress = new Uri("http://localhost:5083") };
    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("mcp-server", "1.0"));
    return client;
});

var app = builder.Build();

await app.RunAsync();

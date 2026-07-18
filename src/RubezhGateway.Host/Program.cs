using System.Diagnostics;
using RubezhGateway.Engine.Middleware;
using RubezhGateway.Host.Provider;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProxyConfigProvider>();

builder.Services.AddSingleton<IProxyConfigProvider>(sp => 
    sp.GetService<ProxyConfigProvider>());

builder.Services.AddReverseProxy()
    .LoadFromMemory(routes: new List<RouteConfig>(), clusters: new List<ClusterConfig>());

var app = builder.Build();

app.UseDefaultMiddleware();

app.MapReverseProxy();

app.Run();

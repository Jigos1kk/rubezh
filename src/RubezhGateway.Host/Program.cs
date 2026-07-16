using System.Diagnostics;
using RubezhGateway.Host.Provider;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProxyConfigProvider>();

builder.Services.AddSingleton<IProxyConfigProvider>(sp => 
    sp.GetService<ProxyConfigProvider>());

builder.Services.AddReverseProxy()
    .LoadFromMemory(routes: new List<RouteConfig>(), clusters: new List<ClusterConfig>());

var app = builder.Build();

app.Use(async (context, next) => {
    var stopwatch = Stopwatch.StartNew();
    context.Request.Headers["X-Rubezh-Gateway"] = "v1.0";
    Console.WriteLine($"[Rubezh] -> {context.Request.Method} {context.Request.Path}");
    await next(context); 
    stopwatch.Stop();
    Console.WriteLine($"[Rubezh] <- {context.Response.StatusCode} | Time: {stopwatch.ElapsedMilliseconds}ms");
});

app.MapReverseProxy();

app.Run();

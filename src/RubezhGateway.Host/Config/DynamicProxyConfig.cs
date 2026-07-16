using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Yarp.ReverseProxy.Configuration;

namespace RubezhGateway.Host.Config
{
    public class InMemoryProxyConfig : IProxyConfig
    {
        public InMemoryProxyConfig(IReadOnlyList<RouteConfig> routes, IReadOnlyList<ClusterConfig> clusters)
        {
            Routes = routes;
            Clusters = clusters;

            _cts = new CancellationTokenSource();
            ChangeToken = new CancellationChangeToken(_cts.Token);
        }

        public IReadOnlyList<RouteConfig> Routes { get; set; }
        public IReadOnlyList<ClusterConfig> Clusters { get; set; }
        public IChangeToken ChangeToken { get; }

        private readonly CancellationTokenSource _cts;

        public void SignalChange()
        {
            _cts.Cancel();   
        }
    }
}
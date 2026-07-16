using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RubezhGateway.Host.Config;
using Yarp.ReverseProxy.Configuration;

namespace RubezhGateway.Host.Provider
{
    public class ProxyConfigProvider : IProxyConfigProvider
    {
        private InMemoryProxyConfig _currentConfig;

        public ProxyConfigProvider()
        {
            _currentConfig = new InMemoryProxyConfig(
                routes: new List<RouteConfig>
                {
                    new RouteConfig
                    {
                        RouteId = "api-route",
                        ClusterId = "backend-api-cluster",
                        Match = new RouteMatch { Path = "/api/{**catch-all}" },
                        Transforms = new List<IReadOnlyDictionary<string, string>>
                        {
                            new Dictionary<string, string> { { "PathRemovePrefix", "/api" } }
                        }
                    }
                },
                clusters: new List<ClusterConfig>
                {
                    new ClusterConfig
                    {
                        ClusterId = "backend-api-cluster",
                        Destinations = new Dictionary<string, DestinationConfig>
                        {
                            { "destination1", new DestinationConfig { Address = "https://github.com/" } }
                        }
                    },
                }
            );
        }

        public IProxyConfig GetConfig() => _currentConfig;

        public void UpdateConfig(IReadOnlyList<RouteConfig> newRoutes, IReadOnlyList<ClusterConfig> newClusters)
        {
            _currentConfig.SignalChange();
            _currentConfig = new InMemoryProxyConfig(newRoutes, newClusters);
        }
    }
}
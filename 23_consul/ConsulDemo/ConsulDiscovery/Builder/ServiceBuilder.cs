using System;
using System.Threading.Tasks;
using ConsulDiscovery.LoadBalancer;

namespace ConsulDiscovery.Builder
{
    public class ServiceBuilder : IServiceBuilder
    {
        public IServiceProvider ServiceProvider { get; set; }
        public string ServiceName { get; set; }
        public string UriScheme { get; set; }
        public ILoadBalancer LoadBalancer { get; set; }

        public ServiceBuilder(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public async Task<Uri> BuilderAsync(string path)
        {
            var serviceList = await ServiceProvider.GetServicesAsync(ServiceName);
            var service = LoadBalancer.Resolve(serviceList);
            var baseUri = new Uri($"{UriScheme}://{service}");
            var uri = new Uri(baseUri, path);
            return uri;
        }
    }
}
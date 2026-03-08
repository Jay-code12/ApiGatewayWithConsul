using Consul;

namespace AuthService.Repositories
{
    public class ConsulServiceDiscovery
    {
        private readonly IConsulClient _consulClient;

        public ConsulServiceDiscovery(IConsulClient consulClient)
        {
            _consulClient = consulClient;
        }

        public async Task<ServiceEntry> GetServiceAsync(string serviceName)
        {
            var services = await _consulClient.Health.Service(serviceName, "", true);

            return services.Response.FirstOrDefault();
        }
    }
}

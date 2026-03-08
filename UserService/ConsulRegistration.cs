using Consul;
using Microsoft.Extensions.Options;
using UserService.Modal.Settings;

namespace UserService
{
    public class ConsulRegistration(IOptions<ConsulSetting> consulSettings)
    {
        private readonly IOptions<ConsulSetting> _consulSettings = consulSettings;

        public void RegistrationService(WebApplication app)
        {
            var consulClient = app.Services.GetRequiredService<IConsulClient>();
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
            //var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");

            var serviceAddress = _consulSettings.Value.ServiceAddress;
            var port = _consulSettings.Value.ServicePort;

            var registration = new AgentServiceRegistration()
            {
                ID = _consulSettings.Value.ServiceId,
                Name = _consulSettings.Value.ServiceName,
                Address = serviceAddress,
                Port = port,

                Check = new AgentServiceCheck()
                {
                    HTTP = $"http://{serviceAddress}:{port}/health",
                    Interval = TimeSpan.FromSeconds(10),
                    Timeout = TimeSpan.FromSeconds(5)
                }
            };

            //logger.LogInformation("Registering with Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();

            lifetime.ApplicationStopping.Register(() =>
            {
                //logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            });
        }
    }
}

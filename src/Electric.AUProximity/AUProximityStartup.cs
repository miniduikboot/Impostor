using Electric.AUProximity.Hub;
using Impostor.Api.Events;
using Impostor.Api.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Electric.AUProximity
{
    public class AUProximityStartup : IPluginStartup
    {
        public void ConfigureHost(IHostBuilder host)
        {
            host.ConfigureWebHostDefaults(builder =>
            {
                builder.Configure(app =>
                {
                    app.UseRouting();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapHub<ProximityHub>("/hub");
                    });
                });
                
                // Required when using the public instance of the Proximity Chat server
                builder.UseUrls("http://0.0.0.0:22044");
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IEventListener, AUProximityListener>();
        }
    }
}

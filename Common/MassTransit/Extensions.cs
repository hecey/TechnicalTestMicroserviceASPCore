using System.Reflection;
using GreenPipes;
using Hecey.TTM.Common.Settings;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hecey.TTM.Common.MassTransit
{
    public static class Extensions{
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services)
        {
            services.AddMassTransit(configure =>
            {
                configure.AddConsumers(Assembly.GetEntryAssembly());
                configure.UsingRabbitMq((context,configurator)=>
                {
                    var configuration = context.GetService<IConfiguration>();
                    var rabbitMQSettings = configuration!.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
                    if(Environment.GetEnvironmentVariable("Host") is not null){
                         rabbitMQSettings = new(){ Host=Environment.GetEnvironmentVariable("Host")};
                     }
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("dev", false));
                    configurator.UseMessageRetry(retryConfiguration => retryConfiguration.Interval(3, TimeSpan.FromSeconds(5)));
                });
            });
            services.AddMassTransitHostedService();
            return services;
        }
    }
}
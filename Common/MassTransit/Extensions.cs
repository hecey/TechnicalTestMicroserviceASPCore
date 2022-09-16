using System.Reflection;
using Hecey.TTM.Common.Settings;
using MassTransit;
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
                    configurator.Host(rabbitMQSettings.Host);
                    configurator.UseMessageRetry(retryConfiguration => retryConfiguration.Interval(3, TimeSpan.FromSeconds(5)));
                });
            });
            return services;
        }
    }
}
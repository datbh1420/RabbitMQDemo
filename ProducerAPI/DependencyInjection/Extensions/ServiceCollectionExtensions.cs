using Contract.Abstraction.Message;
using MassTransit;
using ProducerAPI.DependencyInjection.Options;
using RabbitMQ.Client;

namespace ProducerAPI.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureMassTransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var masstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

            services.AddMassTransit(mt =>
            {
                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(masstransitConfiguration.Host, masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });

                    bus.Message<INotification>(e => e.SetEntityName(masstransitConfiguration.ExchangeName));

                    bus.Publish<INotification>(e =>
                    {
                        e.Durable = true; //Default true
                        e.AutoDelete = false; //Default false
                        e.ExchangeType = ExchangeType.Topic; //Use RabbitMQClient
                    });

                    bus.Send<INotification>(e =>
                    {
                        e.UseRoutingKeyFormatter(context => context.Message.Type.ToString());
                    });

                    bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());
                });
            });
            return services;
        }
    }
}

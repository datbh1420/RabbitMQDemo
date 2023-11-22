using MassTransit;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
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
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(masstransitConfiguration.Host, masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });


                    cfg.Message<DomainEvent.SmsNotification>(x => x.SetEntityName(masstransitConfiguration.ExchangeName));

                    cfg.Publish<DomainEvent.SmsNotification>(x =>
                    {
                        x.ExchangeType = ExchangeType.Topic;
                    });

                    cfg.Send<DomainEvent.SmsNotification>(e =>
                    {
                        e.UseRoutingKeyFormatter(context => context.Message.Type);
                    });
                });
            });
            return services;
        }
    }
}

using ConsumerAPI.DependencyInjection.Options;
using ConsumerAPI.MessageBus.Consumers.Events;
using Contract.Constants;
using MassTransit;
using RabbitMQ.Client;

namespace ConsumerAPI.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediatR(this IServiceCollection services)
            => services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        public static IServiceCollection AddConfigureMassTransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var masstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

            services.AddMassTransit(mt =>
            {

                mt.AddConsumer<SmsNotificationConsumer>();
                mt.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(masstransitConfiguration.Host, masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });

                    cfg.ReceiveEndpoint(masstransitConfiguration.SmsQueueName, ep =>
                    {
                        ep.ConfigureConsumeTopology = false;
                        ep.ConfigureConsumer<SmsNotificationConsumer>(context);
                        ep.Bind(masstransitConfiguration.ExchangeName, s =>
                        {
                            s.RoutingKey = MessageType.sms;
                            s.ExchangeType = ExchangeType.Topic;
                        });
                    });
                    //cfg.ReceiveEndpoint(masstransitConfiguration.SmsQueueName, x =>
                    //{
                    //    x.ConfigureConsumeTopology = false;

                    //    x.SetQuorumQueue();
                    //    x.SetQueueArgument("declare", "lazy");

                    //    x.Consumer<SmsNotificationConsumer>();
                    //    x.Bind(masstransitConfiguration.ExchangeName, s =>
                    //    {
                    //        s.RoutingKey = "sms";
                    //        s.ExchangeType = ExchangeType.Topic;
                    //    });
                    //});
                });
            });
            return services;
        }
    }
}

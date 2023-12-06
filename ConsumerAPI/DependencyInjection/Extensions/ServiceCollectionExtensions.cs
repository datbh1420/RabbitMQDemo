using ConsumerAPI.DependencyInjection.Options;
using ConsumerAPI.MessageBus.Consumers.Events;
using Contract.Constants;
using MassTransit;
using RabbitMQ.Client;
using System.Reflection;

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
                mt.AddConsumers(Assembly.GetExecutingAssembly());

                mt.SetKebabCaseEndpointNameFormatter();

                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(masstransitConfiguration.Host, masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });
                    // SmsNotification
                    bus.ReceiveEndpoint(masstransitConfiguration.SmsQueueName, re =>
                    {
                        re.ConfigureConsumeTopology = false;
                        re.ConfigureConsumer<SmsNotificationConsumer>(context);

                        re.Bind(masstransitConfiguration.ExchangeName, e =>
                        {
                            e.Durable = true;
                            e.AutoDelete = false;
                            e.ExchangeType = ExchangeType.Topic;

                            e.RoutingKey = NotificationType.sms;
                        });
                    });
                    // EmailNotification
                    bus.ReceiveEndpoint(masstransitConfiguration.EmailQueueName, re =>
                    {
                        re.ConfigureConsumeTopology = false;
                        re.ConfigureConsumer<EmailNotificationConsumer>(context);

                        re.Bind(masstransitConfiguration.ExchangeName, e =>
                        {
                            e.Durable = true;
                            e.AutoDelete = false;
                            e.ExchangeType = ExchangeType.Topic;

                            e.RoutingKey = NotificationType.email;
                        });
                    });

                    bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                    bus.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}

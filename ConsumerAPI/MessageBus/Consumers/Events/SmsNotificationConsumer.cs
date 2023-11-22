using MassTransit;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using MediatR;

namespace ConsumerAPI.MessageBus.Consumers.Events
{
    public class SmsNotificationConsumer : IConsumer<DomainEvent.SmsNotification>
    {
        private readonly ISender sender;

        public SmsNotificationConsumer(ISender sender)
        {
            this.sender = sender;
        }
        public async Task Consume(ConsumeContext<DomainEvent.SmsNotification> context)
        => await sender.Send(context.Message);
    }
}

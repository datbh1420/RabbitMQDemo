using ConsumerAPI.Abstraction;
using Contract.IntergrationEvents;
using MediatR;

namespace ConsumerAPI.MessageBus.Consumers.Events
{
    public class SmsNotificationConsumer : Consumer<DomainEvent.SmsNotification>
    {
        public SmsNotificationConsumer(ISender sender) : base(sender)
        {
        }
    }
}

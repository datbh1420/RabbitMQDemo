using ConsumerAPI.Abstraction;
using Contract.IntergrationEvents;
using MediatR;

namespace ConsumerAPI.MessageBus.Consumers.Events
{
    public class EmailNotificationConsumer : Consumer<DomainEvent.EmailNotification>
    {
        public EmailNotificationConsumer(ISender sender) : base(sender)
        {
        }
    }
}

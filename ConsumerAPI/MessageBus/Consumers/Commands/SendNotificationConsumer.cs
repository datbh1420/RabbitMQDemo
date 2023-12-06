using Contract.IntergrationEvents;
using MassTransit;
using MediatR;

namespace ConsumerAPI.MessageBus.Consumers.Commands
{
    public class SendNotificationConsumer : IConsumer<Command.SendNotification>
    {
        private readonly ISender sender;

        public SendNotificationConsumer(ISender sender)
        {
            this.sender = sender;
        }
        public async Task Consume(ConsumeContext<Command.SendNotification> context)
                => await sender.Send(context.Message);
    }
}

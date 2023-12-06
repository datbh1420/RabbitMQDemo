using MassTransit;
using MediatR;

namespace ConsumerAPI.Abstraction
{
    public abstract class Consumer<TMessage> : IConsumer<TMessage>
        where TMessage : class, Contract.Abstraction.Message.INotification
    {
        private readonly ISender Sender;

        protected Consumer(ISender sender)
        {
            Sender = sender;
        }

        public async Task Consume(ConsumeContext<TMessage> context)
            => await Sender.Send(context.Message);
    }
}

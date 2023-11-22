using Contract.Abstraction.Message;
using MassTransit;

namespace Contract.Abstraction
{
    public class Consumer<TMessage> : IConsumer<TMessage>
        where TMessage : class, IMessage
    {
        public Task Consume(ConsumeContext<TMessage> context)
        {
            return Task.CompletedTask;
        }
    }
}

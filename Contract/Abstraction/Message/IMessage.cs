using MassTransit;
using MediatR;

namespace Contract.Abstraction.Message
{
    [ExcludeFromTopology]
    public interface IMessage : IRequest
    {
        public Guid Id { get; set; }
    }
}

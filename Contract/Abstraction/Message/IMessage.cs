using MassTransit;
using MediatR;

namespace Contract.Abstraction.Message
{
    [ExcludeFromTopology]
    public interface IMessage : IRequest
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Type { get; set; }
        public Guid TransactionId { get; set; }
    }
}

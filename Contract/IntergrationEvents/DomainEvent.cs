

using Contract.Abstraction.Message;

namespace MasstTransitRabbitMQ.Contract.IntergrationEvents
{
    public static class DomainEvent
    {
        public record class SmsNotification : IMessage
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Type { get; set; }
            public Guid TransactionId { get; set; }
        }
    }
}

using Contract.IntergrationEvents;
using MediatR;

namespace Services.EmailAPI.UseCases.Events
{
    public class SendSmsNotificationConsumerHandler : IRequestHandler<DomainEvent.SmsNotification>
    {
        public async Task Handle(DomainEvent.SmsNotification request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Content.ToString());
        }
    }
}

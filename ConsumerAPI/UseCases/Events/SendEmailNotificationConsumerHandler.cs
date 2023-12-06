using Contract.IntergrationEvents;
using MediatR;

namespace Services.EmailAPI.UseCases.Events
{
    public class SendEmailNotificationConsumerHandler : IRequestHandler<DomainEvent.EmailNotification>
    {
        public async Task Handle(DomainEvent.EmailNotification request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Content.ToString());
        }
    }
}

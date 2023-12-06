using Contract.IntergrationEvents;
using MediatR;

namespace ConsumerAPI.UseCases.Commands
{
    public class SenNotification : IRequestHandler<Command.SendNotification>
    {
        public async Task Handle(Command.SendNotification request, CancellationToken cancellationToken)
        {
            Console.WriteLine(request.Content.ToString());
        }
    }
}

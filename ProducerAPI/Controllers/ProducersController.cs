using Contract.Constants;
using Contract.IntergrationEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        private readonly IBus bus;
        public ProducersController(IPublishEndpoint publishEndpoint, IBus bus)
        {
            this.publishEndpoint = publishEndpoint; // Not generate queue
            this.bus = bus; // Auto generate queue
        }

        [HttpPost()]
        public async Task<IActionResult> PublishSmsNotification()
        {
            var message = new DomainEvent.SmsNotification
            {
                Id = Guid.NewGuid(),
                Title = "Sms Notification",
                Content = "Content",
                Type = NotificationType.sms,
                TransactionId = Guid.NewGuid(),
            };
            using var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await publishEndpoint.Publish(message, cancelToken.Token);
            return Accepted();
        }

        [HttpPost()]
        public async Task<IActionResult> PublishEmailNotification()
        {
            var message = new DomainEvent.EmailNotification
            {
                Id = Guid.NewGuid(),
                Title = "Email Notification",
                Content = "Content",
                Type = NotificationType.email,
                TransactionId = Guid.NewGuid(),
            };
            using var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await publishEndpoint.Publish(message, cancelToken.Token);
            return Accepted();
        }


        [HttpPost()]
        public async Task<IActionResult> SendNotification()
        {
            var sendNotification = new Command.SendNotification
            {
                Id = Guid.NewGuid(),
                Content = "Content",
                Title = "Message",
                Type = NotificationType.email,
                TransactionId = Guid.NewGuid()
            };
            using var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            var endpoint = await bus.GetSendEndpoint(Address<Command.SendNotification>()); //send-notification
            await endpoint.Send(sendNotification, cancelToken.Token);

            return Accepted();
        }


        private static Uri Address<T>()
            => new($"queue:{KebabCaseEndpointNameFormatter.Instance.SanitizeName(typeof(T).Name)}");


    }
}

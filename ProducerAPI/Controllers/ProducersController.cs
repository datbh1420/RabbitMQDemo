using Contract.Constants;
using MassTransit;
using MasstTransitRabbitMQ.Contract.IntergrationEvents;
using Microsoft.AspNetCore.Mvc;

namespace ProducerAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProducersController : ControllerBase
    {
        private readonly IPublishEndpoint publishEndpoint;
        public ProducersController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpPost()]
        public async Task<IActionResult> PublishSmsNotification()
        {
            var message = new DomainEvent.SmsNotification
            {
                Id = Guid.NewGuid(),
                Title = "Sms Notification",
                Content = "Content",
                Type = MessageType.sms,
                TransactionId = Guid.NewGuid(),
            };
            var cancelToken = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            await publishEndpoint.Publish(message, cancelToken.Token);
            return Accepted();
        }
    }
}

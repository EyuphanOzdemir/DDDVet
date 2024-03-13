using Microsoft.Extensions.Logging;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Models;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using VetClinicPublic.Interfaces;

namespace VetClinicPublic.Web.Services
{
  public class ClientCreatedEmailHandler : IRequestHandler<SendClientCreatedCommand>
  {
    readonly ILogger<ClientCreatedEmailHandler> _logger;
    private readonly ISendClientCreatedEmails _emailSender;

    public ClientCreatedEmailHandler(ILogger<ClientCreatedEmailHandler> logger,
      ISendClientCreatedEmails emailSender)
    {
      _logger = logger;
      _emailSender = emailSender;
    }


    public Task<Unit> Handle(SendClientCreatedCommand request, CancellationToken cancellationToken)
    {
      _logger.LogInformation("Message Received - Sending Email!");

      _emailSender.SendClientCreatedEmail(request);

      return Task.FromResult(Unit.Value);
    }
  }
}

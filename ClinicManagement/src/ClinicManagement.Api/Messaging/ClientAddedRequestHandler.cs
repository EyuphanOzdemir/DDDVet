using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using ClinicManagement.Api.ApplicationEvents;
using ClinicManagement.Core.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace ClinicManagement.Api.Messaging
{
  public class ClientAddedRequestHandler : IRequestHandler<ClientAddedRequest>
  {
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<ClientAddedRequestHandler> _logger;
    private readonly IMapper _mapper;
    public ClientAddedRequestHandler(ILogger<ClientAddedRequestHandler> logger, IMessagePublisher messagePublisher, IMapper mapper) 
    {
      _messagePublisher = messagePublisher;
      _logger = logger;
      _mapper = mapper;
    }
    public Task<Unit> Handle(ClientAddedRequest request, CancellationToken cancellationToken)
    {
      var appEvent = new EntityWithEmailCreatedEvent(_mapper.Map<EntityWithEmail>(request), "Client-Created");
      _logger.LogInformation("Sending client created event: {0}", appEvent);
      _messagePublisher.Publish(appEvent);

      return Task.FromResult(Unit.Value);
    }
  }
}

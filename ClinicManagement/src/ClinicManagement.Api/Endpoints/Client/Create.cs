using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Client;
using ClinicManagement.Api.ApplicationEvents;
using ClinicManagement.Api.Messaging;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.ClientEndpoints
{
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreateClientRequest>
    .WithResponse<CreateClientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;


    public Create(IRepository<Client> repository, IMapper mapper, IMediator mediator, IMessagePublisher messagePublisher,  ILogger<Create> logger)
    {
      _repository = repository;
      _mapper = mapper;
      _mediator = mediator;
    }

    [HttpPost("api/clients")]
    [SwaggerOperation(
        Summary = "Creates a new Client",
        Description = "Creates a new Client",
        OperationId = "clients.create",
        Tags = new[] { "ClientEndpoints" })
    ]
    public override async Task<ActionResult<CreateClientResponse>> HandleAsync(CreateClientRequest request, CancellationToken cancellationToken)
    {
      var response = new CreateClientResponse(request.CorrelationId);

      var toAdd = _mapper.Map<Client>(request);
      toAdd = await _repository.AddAsync(toAdd);

      var dto = _mapper.Map<ClientDto>(toAdd);
      response.Client = dto;

      await _mediator.Send(new ClientAddedRequest { FullName = dto.FullName, EmailAddress = dto.EmailAddress }, CancellationToken.None);
      return Ok(response);
    }
  }
}

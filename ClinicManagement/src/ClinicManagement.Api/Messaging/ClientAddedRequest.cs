using MediatR;

namespace ClinicManagement.Api.Messaging
{
  public class ClientAddedRequest:IRequest
  {
    public string FullName { get; set; }
    public string EmailAddress { get; set; }
  }
}

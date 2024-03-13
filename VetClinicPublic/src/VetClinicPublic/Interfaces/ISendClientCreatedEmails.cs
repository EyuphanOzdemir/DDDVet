using VetClinicPublic.Web.Models;

namespace VetClinicPublic.Interfaces
{
  public interface ISendClientCreatedEmails
  {
    void SendClientCreatedEmail(SendClientCreatedCommand clientCreatedCommand);
  }
}

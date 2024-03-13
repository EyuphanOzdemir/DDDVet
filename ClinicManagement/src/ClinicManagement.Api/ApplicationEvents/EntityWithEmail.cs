using System.Text.Json.Serialization;

namespace ClinicManagement.Api.ApplicationEvents
{
  public class EntityWithEmail
  {
    public string FullName { get; set; }
    public string EmailAddress { get; set; }
  }
}

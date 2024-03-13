using AutoMapper;
using ClinicManagement.Api.ApplicationEvents;
using ClinicManagement.Api.Messaging;
using ClinicManagement.Core.Aggregates;

namespace ClinicManagement.Api.MappingProfiles
{
  public class EntityWithEmailProfile:Profile
  {
    public EntityWithEmailProfile()
    {
      CreateMap<ClientAddedRequest, EntityWithEmail>();
    }
  }
}

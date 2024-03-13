using PluralsightDdd.SharedKernel.Interfaces;

namespace ClinicManagement.Api.ApplicationEvents
{
  public class EntityWithEmailCreatedEvent : IApplicationEvent
  {
    public string EventType { get; set; }
    public EntityWithEmail Entity { get; set; }

    public EntityWithEmailCreatedEvent(EntityWithEmail entity, string eventType)
    {
      Entity = entity;
      EventType = eventType;
    }
  }
}

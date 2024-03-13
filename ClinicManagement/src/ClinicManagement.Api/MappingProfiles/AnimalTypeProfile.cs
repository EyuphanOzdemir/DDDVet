using AutoMapper;
using BlazorShared.Models.Patient;
using ClinicManagement.BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.ValueObjects;

namespace ClinicManagement.Api.MappingProfiles
{
  public class AnimalTypeProfile:Profile
  {
    public AnimalTypeProfile()
    {
      CreateMap<AnimalTypeDto, AnimalType>();
      CreateMap<AnimalType, AnimalTypeDto>();
    }
  }
}

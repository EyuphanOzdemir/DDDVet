using AutoMapper;
using BlazorShared.Models.Patient;
using ClinicManagement.BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.ValueObjects;

namespace ClinicManagement.Api.MappingProfiles
{
  public class PatientProfile : Profile
  {
    public PatientProfile()
    {
      CreateMap<Patient, PatientDto>()
          .ForMember(dto => dto.PatientId, options => options.MapFrom(src => src.Id))
          .ForMember(dto => dto.ClientName, options => options.MapFrom(src => string.Empty));
      CreateMap<PatientDto, Patient>()
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.PatientId));
      CreateMap<Patient, int>().ConvertUsing(src => src.Id);
      CreateMap<CreatePatientRequest, Patient>();
      CreateMap<UpdatePatientRequest, Patient>()
          .ForMember(dto => dto.Id, options => options.MapFrom(src => src.PatientId));
      CreateMap<DeletePatientRequest, Patient>()
        .ForMember(dto => dto.Id, options => options.MapFrom(src => src.PatientId))
        .ForMember(dto => dto.ClientId, options => options.MapFrom(src => src.ClientId));
    }
  }
}

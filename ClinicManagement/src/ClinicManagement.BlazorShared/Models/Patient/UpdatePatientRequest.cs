using ClinicManagement.BlazorShared.Models.Patient;

namespace BlazorShared.Models.Patient
{
  public class UpdatePatientRequest : BaseRequest
  {
    public int ClientId { get; set; }
    public int PatientId { get; set; }

    public string PatientName { get; set; }
    public AnimalTypeDto AnimalType { get; set; }
  }
}

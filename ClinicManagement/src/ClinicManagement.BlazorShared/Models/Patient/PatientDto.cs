using ClinicManagement.BlazorShared.Models.Patient;

namespace BlazorShared.Models.Patient
{
  public class PatientDto
  {
    public int PatientId { get; set; }
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string ClientName { get; set; }
    public string ImageData { get; set; }

    public string PictureUrl { get; set; }
    public int? PreferredDoctorId { get; set; }

    public string PreferredDoctorName { get;}

    public AnimalTypeDto AnimalType { get; set; }
  }
}

using System.ComponentModel.DataAnnotations;
using ClinicManagement.Core.CustomValidationAttributes;
using Microsoft.AspNetCore.Http;
namespace ClinicManagement.Core.CustomModelBindings
{
  public class PatientUpdatePictureModel
  {
    [Required(ErrorMessage = "File is required.")]
    [AllowedImageExtensions(".jpg", ".png", ".gif", ErrorMessage = "Invalid file format. Only JPG, JPEG, and PNG are allowed.")]
    [MaxFileSize(5 * 1024 * 1024, ErrorMessage = "File size exceeds the maximum allowed (5 MB).")]
    public IFormFile File { get; set; }
    public string Destination { get; set; }
    public int ClientID { get; set; }
    public int PatientId { get; set; }
  }
}

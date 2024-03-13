using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace ClinicManagement.Core.CustomValidationAttributes
{
  public class AllowedImageExtensionsAttribute : ValidationAttribute
  {
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };


    public AllowedImageExtensionsAttribute()
    {

    }
    public AllowedImageExtensionsAttribute(params string[] allowedExtensions)
    {
      _allowedExtensions = allowedExtensions;
    }
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is IFormFile file)
      {
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (_allowedExtensions.Contains(fileExtension))
        {
          return ValidationResult.Success;
        }
        else
        {
          return new ValidationResult("Invalid file format. Only JPG, JPEG, and PNG are allowed.");
        }
      }

      return new ValidationResult("Invalid file format.");
    }
  }
}

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ClinicManagement.Core.CustomValidationAttributes
{
  public class MaxFileSizeAttribute:ValidationAttribute
  {
    private readonly long _maxFileSize;

    public MaxFileSizeAttribute(long maxFileSize)
    {
      _maxFileSize = maxFileSize;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value is IFormFile file)
      {
        if (file.Length <= _maxFileSize)
        {
          return ValidationResult.Success;
        }
        else
        {
          return new ValidationResult($"File size exceeds the maximum allowed ({_maxFileSize / (1024 * 1024)} MB).");
        }
      }

      return new ValidationResult("Invalid file.");
    }
  }
}

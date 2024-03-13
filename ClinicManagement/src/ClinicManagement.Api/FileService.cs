using ClinicManagement.Core.CustomModelBindings;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace ClinicManagement.Api
{
  public class FileService
  {
    public async Task<bool> PutFile(IFormFile file, string destination)
    {
      var fullPath = Path.Combine(Directory.GetCurrentDirectory(), destination, file.FileName.ToLower());
      if (System.IO.File.Exists(fullPath))
      {
        System.IO.File.Delete(fullPath);
      }

      using (var stream = new FileStream(fullPath, FileMode.Create))
      {
        await file.CopyToAsync(stream);
      }

      return true;
    }

  }
}

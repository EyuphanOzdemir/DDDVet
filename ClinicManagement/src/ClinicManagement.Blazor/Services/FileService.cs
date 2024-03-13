using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorShared.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace ClinicManagement.Blazor.Services
{
  public class FileService
  {
    private readonly HttpService _httpService;

    public FileService(HttpService httpService)
    {
      _httpService = httpService;
    }

    public async Task<string> ReadPicture(string pictureName)
    {
      if (string.IsNullOrEmpty(pictureName))
      {
        return null;
      }
      var fileItem = await _httpService.HttpGetAsync<FileItem>($"files/{pictureName}");

      return fileItem == null ? null : fileItem.DataBase64;
    }


    public MultipartFormDataContent BuildFormData(IBrowserFile file, Dictionary<string,string> contentItems)
    {
      var formData = new MultipartFormDataContent();

      using (var stream = file.OpenReadStream())
      {
        formData.Add(new StreamContent(stream), "File", file.Name);
      }
      
      foreach (var item in contentItems)
      {
        formData.Add(new StringContent(item.Value), item.Key);
      }

      return formData;
      
    }
  }
}

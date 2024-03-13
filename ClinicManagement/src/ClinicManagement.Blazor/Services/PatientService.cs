using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorShared.Models.Patient;
using ClinicManagement.Blazor.Utilities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Blazor.Services
{
  public class PatientService
  {
    private readonly HttpService _httpService;
    private readonly FileService _fileService;
    private readonly ILogger<PatientService> _logger;

    public PatientService(HttpService httpService, ILogger<PatientService> logger, FileService fileService)
    {
      _httpService = httpService;
      _logger = logger;
      _fileService = fileService;
    }

    public async Task<PatientDto> CreateAsync(CreatePatientRequest patient)
    {
      return (await _httpService.HttpPostAsync<CreatePatientResponse>("patients", patient)).Patient;
    }

    public async Task<PatientDto> EditAsync(UpdatePatientRequest patient)
    {
      return (await _httpService.HttpPutAsync<UpdatePatientResponse>("patients", patient)).Patient;
    }

    public Task DeleteAsync(int clientId, int patientId)
    {
      return _httpService.HttpDeleteAsync<DeletePatientResponse>("patients", new Dictionary<string, int>() { { "client", clientId }, { "patient", patientId } });
    }

    public async Task<PatientDto> GetByIdAsync(int patientId)
    {
      return (await _httpService.HttpGetAsync<GetByIdPatientResponse>($"patients/{patientId}")).Patient;
    }

    public async Task<List<PatientDto>> ListPagedAsync(int pageSize)
    {
      _logger.LogInformation("Fetching patients from API.");

      return (await _httpService.HttpGetAsync<ListPatientResponse>($"patients")).Patients;
    }

    public async Task<List<PatientDto>> ListAsync(int clientId)
    {
      _logger.LogInformation("Fetching patients from API.");

      return (await _httpService.HttpGetAsync<ListPatientResponse>($"patients?ClientId={clientId}")).Patients;
    }

    public async Task<UpdatePatientResponse> PostPicture(IBrowserFile file, UpdatePatientRequest patient)
    {
      var contentItems = new Dictionary<string, string>() { { "Destination", Utility.PatientPictureFolderToUpload }, {"ClientId", patient.ClientId.ToString()}, { "PatientId", patient.PatientId.ToString() } };
      var formData = _fileService.BuildFormData(file, contentItems);
      return await _httpService.HttpPostFormAsync<UpdatePatientResponse>(@"patients/update_picture", formData);
    }
  }
}

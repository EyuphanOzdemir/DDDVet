using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorShared.Models.Client;
using BlazorShared.Models.Patient;
using ClinicManagement.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using ClinicManagement.BlazorShared.Models.Patient;
using System;
using Microsoft.AspNetCore.Components.Forms;

namespace ClinicManagement.Blazor.Pages
{
  public partial class PatientsPage
  {
    [Inject]
    IJSRuntime JSRuntime { get; set; }

    [Inject]
    ClientService ClientService { get; set; }
    [Inject]
    PatientService PatientService { get; set; }

    [Inject]
    FileService FileService { get; set; }

    private IBrowserFile selectedFile;
    private string uploadStatusMessage = "";
    private void HandleFileChange(InputFileChangeEventArgs e) {
      selectedFile = e.File; 
    }

    private PatientDto   ToSave = new();
    private List<ClientDto> Clients = new ();
    private List<PatientDto> Patients = new();
    // Selected client
    private ClientDto SelectedClient;

    // Handle the client selection change
    private async void OnClientSelectionChanged(ChangeEventArgs e)
    {
      // Find the selected client by its name
      SelectedClient = Clients.FirstOrDefault(c => c.FullName == e.Value.ToString());

      // Now you can use selectedClient.Id to retrieve patients
      // You may want to perform the necessary logic here to fetch patients
      // For example, call a method to load patients based on the selected client
      if (SelectedClient != null)
      {
        Patients = await LoadPatientsForSelectedClient();
        StateHasChanged();
      }
    }

    // Method to load patients for the selected client
    private async Task<List<PatientDto>> LoadPatientsForSelectedClient()
    {
      return await PatientService.ListAsync(SelectedClient.ClientId);
    }

    protected override async Task OnInitializedAsync()
    {
      await ReloadClientsData();
    }

    private void CreateClick()
    {
      if (SelectedClient!.ClientId > 0)
      {
        if (Patients.Count == 0 || Patients[Patients.Count - 1].PatientId != 0)
        {
          ToSave = new PatientDto();
          ToSave.ClientId = SelectedClient.ClientId;
          if (ToSave.AnimalType == null)
          {
            ToSave.AnimalType = new AnimalTypeDto();
          }
          Patients.Add(ToSave);
        }
      }
    }

    private void EditClick(int id)
    {
      ToSave = Patients.Find(x => x.PatientId == id);
      if (ToSave.AnimalType == null)
      {
        ToSave.AnimalType = new AnimalTypeDto();
      }
    }

    private async Task DeleteClick(int id)
    {
      bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure?");
      if (confirmed)
      {
        await PatientService.DeleteAsync(SelectedClient.ClientId, id);
        Patients = await LoadPatientsForSelectedClient();
      }
    }

    private async Task SaveClick()
    {
      if (ToSave.PatientId == 0)
      {
        var toCreate = new CreatePatientRequest()
        {
          PatientName = ToSave.Name,
          ClientId = SelectedClient.ClientId,
          AnimalType = ToSave.AnimalType
        };
        await PatientService.CreateAsync(toCreate);
      }
      else
      {
        var toUpdate = new UpdatePatientRequest()
        {
          ClientId = SelectedClient.ClientId,
          PatientId = ToSave.PatientId,
          PatientName = ToSave.Name,
          AnimalType = ToSave.AnimalType,
        };

        await PatientService.EditAsync(toUpdate);
      }

      CancelClick();
      Patients = await LoadPatientsForSelectedClient(); 
      StateHasChanged();
    }

    private void CancelClick()
    {
      if (ToSave.PatientId == 0)
      {
        Patients.RemoveAt(Patients.Count - 1);
      }
      ToSave = new PatientDto();
    }

    private bool InEditCreateMode(int id)
    {
      return ToSave.PatientId == id;
    }

    private async Task ReloadClientsData()
    {
      Clients = await ClientService.ListAsync();
    }

    private async void UploadPicture(int id)
    {
      if (selectedFile != null)
      {
        var response = await PatientService.PostPicture(selectedFile, new UpdatePatientRequest() {ClientId = SelectedClient.ClientId, PatientId =  id});
        if (response == null || response.Patient.PatientId!=id || response.Patient.PictureUrl == null)
        {
           uploadStatusMessage = "Upload failed!";
        }
        else
        {
          uploadStatusMessage = "Upload successful!";
          Patients = await LoadPatientsForSelectedClient();
        }
      }
      else
      {
        uploadStatusMessage = "Please select a file to upload.";
      }
      StateHasChanged();
    }

    private async Task<List<PatientDto>> UpdatePatientIamge()
    {
      return await PatientService.ListAsync(SelectedClient.ClientId);
    }
  }
}

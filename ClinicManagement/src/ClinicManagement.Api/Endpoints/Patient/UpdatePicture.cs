using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models;
using BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.CustomModelBindings;
using ClinicManagement.Core.Specifications;
using ClinicManagement.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.Endpoints.Patient
{
  public class UpdatePicture : BaseAsyncEndpoint
    .WithRequest<PatientUpdatePictureModel>
    .WithResponse<UpdatePatientResponse>
  {
    private readonly IRepository<Client> _repository;
    private readonly IMapper _mapper;
    private readonly FileService _fileService;

    public UpdatePicture(IRepository<Client> repository, IMapper mapper, FileService fileService)
    {
      _repository = repository;
      _mapper = mapper;
      _fileService = fileService;
    }

    [HttpPost("api/patients/update_picture")]
    [SwaggerOperation(
    Summary = "Uploads a file from a form",
    Description = "Uploads a file from a form",
    OperationId = "files.upload.form",
    Tags = new[] { "FileEndpoints" })]

    public override async Task<ActionResult<UpdatePatientResponse>> HandleAsync([FromForm] PatientUpdatePictureModel model, CancellationToken cancellationToken)
    {
      //Update the picture url
      var response = new UpdatePatientResponse(new Guid());
      // Check if the model is valid based on the attributes
      if (!ModelState.IsValid)
      {
        // Return the validation errors
        return BadRequest(false);
      }

      await _fileService.PutFile(model.File, model.Destination);


      var spec = new ClientByIdIncludePatientsSpec(model.ClientID);
      var client = await _repository.GetBySpecAsync(spec);
      if (client == null) return NotFound();


      var patientToUpdate = client.Patients.FirstOrDefault(p => p.Id == model.PatientId);
      if (patientToUpdate == null) return NotFound();
      patientToUpdate.PictureUrl = model.File.FileName;
      await _repository.UpdateAsync(client);

      var dto = _mapper.Map<PatientDto>(patientToUpdate);
      response.Patient = dto;

      return Ok(response);
    }
  }
}

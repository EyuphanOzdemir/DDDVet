using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.PatientEndpoints
{
  public class GetPatientImage : BaseAsyncEndpoint
    .WithRequest<GetPatientImageRequest>
    .WithResponse<GetPatientImageResponse>
  {
    private readonly IMapper _mapper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GetPatientImage(IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
      _mapper = mapper;
      _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("api/patients/images/{Filename}")]
    [SwaggerOperation(
        Summary = "Get Patient image",
        Description = "Get Patient image",
        OperationId = "patients.GetPatientImage",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<GetPatientImageResponse>> HandleAsync([FromRoute] GetPatientImageRequest request, CancellationToken cancellationToken)
    {
      try
      {
        var filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "images", "patients", request.Filename);

        if (System.IO.File.Exists(filePath))
        {
          var contentType = GetContentType(request.Filename);

          return PhysicalFile(filePath, contentType);
        }
        else
        {
          return NotFound();
        }
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Image could not be found or served: {ex.Message}");
      }
    }

    private string GetContentType(string fileName)
    {
      var provider = new FileExtensionContentTypeProvider();
      if (!provider.TryGetContentType(fileName, out var contentType))
      {
        contentType = "application/octet-stream"; // Default to binary if the content type is not recognized
      }

      return contentType;
    }
  }
}

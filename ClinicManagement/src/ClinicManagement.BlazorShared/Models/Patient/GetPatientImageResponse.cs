using System;
using System.Collections.Generic;

namespace BlazorShared.Models.Patient
{
  public class GetPatientImageResponse : BaseResponse
  {

    public GetPatientImageResponse(Guid correlationId) : base(correlationId)
    {
    }

    public GetPatientImageResponse()
    {
    }
  }
}

using System;
using MediatR;

namespace VetClinicPublic.Web.Models
{
    public class SendClientCreatedCommand : IRequest
    {
        public string ClientName { get; set; }
        public string ClientEmailAddress { get; set; }
    }
}

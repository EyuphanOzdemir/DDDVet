using System;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VetClinicPublic.Interfaces;
using VetClinicPublic.Web.Interfaces;
using VetClinicPublic.Web.Models;

namespace VetClinicPublic.Web.Services
{
  /// <summary>
  /// Emails are sent from the public site because that's where the SMTP server is in this example.
  /// This is mainly just to demonstrate that messages can pass both to and from this system and other systems.
  /// </summary>
  public class ClientCreatedEmailSender : ISendClientCreatedEmails
  {
    private readonly ISendEmail _emailSender;
    private readonly ILogger<ClientCreatedEmailSender> _logger;
    private readonly SiteConfiguration _config;

    public ClientCreatedEmailSender(ISendEmail emailSender,
      ILogger<ClientCreatedEmailSender> logger,
      IOptions<SiteConfiguration> settings)
    {
      _emailSender = emailSender;
      _logger = logger;
      _config = Guard.Against.Null(settings.Value, nameof(settings));
    }

    public void SendConfirmationEmail(Models.SendAppointmentConfirmationCommand appointment)
    {
      _logger.LogInformation($"Sending email to confirm appointment: {appointment}");
      string confirmUrl = $"http://localhost:{_config.Port}/appointment/confirm/{appointment.AppointmentId}";
      string to = appointment.ClientEmailAddress;
      string from = "donotreply@thevetclinic.com";
      string subject = "Vet Appointment Confirmation for " + appointment.PatientName;
      string body = String.Format("<html><body>Dear {0},<br/><p>Please click the link below to confirm {1}'s appointment for a {2} with {3} on {4}.</p><p>Thanks!</p><p><a href='{5}'>CONFIRM</a></p><p>Please call the office to reschedule if you will be unable to make it for your appointment.</p><p>Have a great day!</p></body></html>", appointment.ClientName, appointment.PatientName, appointment.AppointmentType, appointment.DoctorName, appointment.AppointmentStartDateTime.ToString(), confirmUrl);

      _emailSender.SendEmail(to, from, subject, body);
    }

    public void SendClientCreatedEmail(SendClientCreatedCommand clientCreatedCommand)
    {
      _logger.LogInformation($"Sending email to confirm appointment: {clientCreatedCommand}");
      string to = clientCreatedCommand.ClientEmailAddress;
      string from = "donotreply@thevetclinic.com";
      string subject = "Welcome!";
      string body = String.Format($"<html><body>Welcome {clientCreatedCommand.ClientName}</body></html>");

      _emailSender.SendEmail(to, from, subject, body);
    }
  }
}

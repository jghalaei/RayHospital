using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RayHospital.App.Consultations;
using RayHospital.App.Services;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Infrastructure;
using RayHospital.Infrastructure.Data;
using RayHospital.Infrastructure.Repositories;
using RayHospital.Interfaces;
using RayHospital.Resources;

namespace RayHospital.App
{
  class Program
  {
    /// This is a test application for the RayHospital booking system.
    /// It will simulate a number of patient registrations happening on a number of days starting from today, and print the resulting consultations.
    /// In a real application, the BookConsultation() and Consultations() methods would probably be called from controllers in a REST API.

    static void Main(string[] args)
    {
      var host = Host.CreateDefaultBuilder(args)
                  .ConfigureServices((context, services) =>
                    {
                      services.AddInfrastructureServices();
                      services.AddScoped<TreatmentRoomServices>();
                      services.AddScoped<DoctorServices>();
                      services.AddScoped<IConsultationsManager, ConsultationsManager>();
                    }
                  ).Build();

      DataSeeder.SeedData();
      // TODO: Create implementation of IConsultationsManager and initialize using HospitalResources.
      IConsultationsManager consultationsManager = host.Services.GetService<IConsultationsManager>();

      // Read hard-coded list of patients and book a consultation for each patient
      DateTime startDate = DateTime.Today;
      foreach (var registration in PatientRegistrations.Patients)
      {
        var registrationDate = startDate.AddDays(registration.OffsetDays);
        var patient = new PatientRegistrationModel { Name = registration.Name, Condition = registration.Condition, Topography = registration.Topography };
        var consultation = consultationsManager.BookConsultation(registrationDate, patient);
      }

      // Print the resulting consultations
      var consultations = consultationsManager.Consultations();
      Console.WriteLine(JsonConvert.SerializeObject(consultations, Formatting.Indented, new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd" }));

    }


  }
}

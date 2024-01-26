using System;
using System.Collections.Generic;
using System.Linq;
using RayHospital.App.Mappers;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Managers;

public class ConsultationsManager : IConsultationsManager
{
    private readonly IRepository<Consultation> _consultationRepository;
    private readonly IRoomsManager _roomServices;
    private readonly IDoctorsManager _doctorServices;

    public ConsultationsManager(IRepository<Consultation> consultationRepository,
                                IRoomsManager roomServices,
                                IDoctorsManager doctorServices)
    {
        _consultationRepository = consultationRepository;
        _roomServices = roomServices;
        _doctorServices = doctorServices;
    }

    public ConsultationModel BookConsultation(DateTime registrationDate, PatientRegistrationModel patient)
    {
        registrationDate = registrationDate.Date;
        var book = FindFirstAvailableConsultationTime(registrationDate, patient);
        _consultationRepository.Insert(book);
        return book.ToConsultationModel();
    }
    public IEnumerable<ConsultationModel> Consultations()
    {
        return _consultationRepository.GetAll().Select(x => x.ToConsultationModel());
    }
    private Consultation FindFirstAvailableConsultationTime(DateTime registrationDate, PatientRegistrationModel patient)
    {

        EConditionType condition = Enum.Parse<EConditionType>(patient.Condition);
        ETopography? topography = Enum.TryParse<ETopography>(patient.Topography, out var result) ? result : null;
        TreatmentRoom room;
        Doctor doctor;
        for (var bookDate = registrationDate; true; bookDate = bookDate.AddDays(1))
        {
            room = _roomServices.GetAvailableRoom(bookDate, condition, topography);
            if (room == null)
                continue;
            doctor = _doctorServices.GetAvailableDoctors(bookDate, condition);
            if (doctor == null)
                continue;
            return new Consultation(registrationDate, patient.Name, doctor.Name, room.Name, bookDate);
        }
    }


}

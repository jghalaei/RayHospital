using System;
using System.Linq;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Managers;
public class DoctorsManager : IDoctorsManager
{
    private readonly IRepository<Doctor> _doctorRepository;
    private readonly IRepository<Consultation> _consultationRepository;

    public DoctorsManager(IRepository<Doctor> doctorRepository, IRepository<Consultation> consultationRepository)
    {
        _doctorRepository = doctorRepository;
        _consultationRepository = consultationRepository;
    }


    public Doctor GetAvailableDoctors(DateTime date, EConditionType condition)
    {
        foreach (var doctor in _doctorRepository.GetAll(x => IsDoctorMatched(x, condition)))
        {
            if (CheckDoctorAvailability(date, doctor))
                return doctor;
        }
        return null;
    }

    private bool CheckDoctorAvailability(DateTime date, Doctor doctor)
    {
        var book = _consultationRepository.GetOne(c => c.ConsultaionDate == date && c.DoctorName == doctor.Name);
        return book == null;
    }

    private bool IsDoctorMatched(Doctor x, EConditionType condition)
    {
        string consideredRole = condition == EConditionType.Cancer ? "Oncologist" : "GeneralPractitioner";
        return x.Roles.Contains(consideredRole);
    }
}
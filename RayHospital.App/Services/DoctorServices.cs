using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Services
{
    public class DoctorServices
    {
        private readonly IRepository<Doctor> _doctorRepository;
        private readonly IRepository<Consultation> _consultationRepository;

        public DoctorServices(IRepository<Doctor> doctorRepository, IRepository<Consultation> consultationRepository)
        {
            _doctorRepository = doctorRepository;
            _consultationRepository = consultationRepository;
        }


        public Doctor GetAvailableDoctor(DateTime date, EConditionType condition)
        {
            foreach (var doctor in _doctorRepository.GetAll().Where(x => IsDoctorMatched(x, condition)))
            {
                if (checkDoctorAvailability(date, doctor))
                    return doctor;
            }
            return null;
        }

        private bool checkDoctorAvailability(DateTime date, Doctor doctor)
        {
            var lastbooked = _consultationRepository.GetAll().LastOrDefault(x => x.DoctorName == doctor.Name);
            return lastbooked == null || lastbooked.ConsultaionDate < date;
        }

        private bool IsDoctorMatched(Doctor x, EConditionType condition)
        {
            string consideredRole = condition == EConditionType.Cancer ? "Oncologist" : "GeneralPractitioner";
            return x.Roles.Contains(consideredRole);
        }
    }
}
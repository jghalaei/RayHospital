using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RayHospital.App.Services;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Consultations
{
    public class ConsultationsManager : IConsultationsManager
    {
        private readonly IRepository<Consultation> _consultationRepository;
        private readonly TreatmentRoomServices _roomServices;
        private readonly DoctorServices _doctorServices;

        public ConsultationsManager(IRepository<Consultation> consultationRepository,
                                    TreatmentRoomServices roomServices,
                                    DoctorServices doctorServices)
        {
            _consultationRepository = consultationRepository;
            _roomServices = roomServices;
            _doctorServices = doctorServices;
        }

        public ConsultationModel BookConsultation(DateTime registrationDate, PatientRegistrationModel patient)
        {
            var book = FindFirstBookTime(registrationDate, patient);
            _consultationRepository.Insert(book);
            return new ConsultationModel
            {
                Date = book.ConsultaionDate,
                PatientName = book.PatientName,
                DoctorName = book.DoctorName,
                RoomName = book.RoomName
            };
        }
        private Consultation FindFirstBookTime(DateTime registrationDate, PatientRegistrationModel patient)
        {

            EConditionType condition = Enum.Parse<EConditionType>(patient.Condition);
            ETopography? topography = Enum.TryParse<ETopography>(patient.Topography, out var result) ? result : null;
            var bookDate = registrationDate;
            for (int days = 0; true; days++)
            {
                bookDate = bookDate.AddDays(days);
                var room = _roomServices.GetAvailableRoom(bookDate, condition, topography);
                if (room == null)
                    continue;
                var doctor = _doctorServices.GetAvailableDoctor(bookDate, condition);
                if (doctor == null)
                    continue;
                return new Consultation(registrationDate, patient.Name, doctor.Name, room.Name, bookDate);
            }
        }

        public IEnumerable<ConsultationModel> Consultations()
        {
            List<ConsultationModel> result = new List<ConsultationModel>();
            foreach (var consultation in _consultationRepository.GetAll())
            {
                result.Add(new ConsultationModel
                {
                    Date = consultation.ConsultaionDate,
                    RoomName = consultation.RoomName,
                    DoctorName = consultation.DoctorName,
                    PatientName = consultation.PatientName
                });
            }
            return result;
        }
    }
}

using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Mappers
{
    public static class ConsultationMapperExtensions
    {
        public static ConsultationModel ToConsultationModel(this Consultation consultation)
        {
            return new ConsultationModel()
            {
                Date = consultation.ConsultaionDate,
                RoomName = consultation.RoomName,
                DoctorName = consultation.DoctorName,
                PatientName = consultation.PatientName
            };
        }
    }
}
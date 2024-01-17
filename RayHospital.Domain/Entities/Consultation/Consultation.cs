using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RayHospital.Domain.Entities.Consultation
{
    public class Consultation
    {
        public Consultation(DateTime registerationDate, Patient patient, Doctor doctor, TreatmentRoom room)
        {
            RegisterationDate = registerationDate;
            Patient = patient;
            Doctor = doctor;
            Room = room;

        }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public TreatmentRoom Room { get; set; }
        public DateTime RegisterationDate { get; set; }
        public DateTime ConsultaionDate { get; set; }
    }
}
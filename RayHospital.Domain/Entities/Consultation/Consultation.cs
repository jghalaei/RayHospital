using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RayHospital.Domain.Entities.Consultation
{
    public class Consultation
    {
        public Consultation(DateTime registerationDate, string patientName, string doctorName, string roomName, DateTime consultaionDate)
        {
            this.PatientName = patientName;
            this.DoctorName = doctorName;
            this.RegisterationDate = registerationDate;
            RoomName = roomName;
            ConsultaionDate = consultaionDate;
        }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string RoomName { get; set; }
        public DateTime RegisterationDate { get; set; }
        public DateTime ConsultaionDate { get; set; }
    }
}
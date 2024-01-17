namespace RayHospital.Domain.Entities
{
    public class TreatmentRoom : HospitalEntity
    {
        public TreatmentMachine? Machine { get; set; }

        public TreatmentRoom(string name, TreatmentMachine? machine = null) : base(name)
        {
            Machine = machine;
        }


    }
}
namespace RayHospital.Domain.Entities
{
    public class TreatmentMachine : HospitalEntity
    {
        public TreatmentMachine(string name, EMachineCapacity capacity) : base(name)
        {
            Capacity = capacity;
        }

        public EMachineCapacity Capacity { get; set; }
    }
}
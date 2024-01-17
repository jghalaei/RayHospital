namespace RayHospital.Domain.Entities
{
    public class TreatmentMachine : HospitalEntity
    {
        public TreatmentMachine(string name, EMachineCapability capability) : base(name)
        {
            Capability = capability;
        }

        public EMachineCapability Capability { get; set; }
    }
}
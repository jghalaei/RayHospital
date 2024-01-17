namespace RayHospital.Domain.Entities
{
    public class Patient : HospitalEntity
    {
        public Patient(string Name, EConditionType condition, ETopography? topography) : base(Name)
        {
            Condition = condition;
            Topography = topography;
        }
        EConditionType Condition { get; set; }
        ETopography? Topography { get; set; }
    }
}
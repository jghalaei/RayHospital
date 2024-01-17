namespace RayHospital.Domain.Entities;

public class Doctor : HospitalEntity
{
    public List<DoctorRole> Roles { get; set; }
    public Doctor(string name, List<DoctorRole> roles) : base(name)
    {
        Roles = roles;
    }
}
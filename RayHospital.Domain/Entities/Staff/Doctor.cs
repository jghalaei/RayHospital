namespace RayHospital.Domain.Entities;

public class Doctor : HospitalEntity
{
    public string[] Roles { get; set; }
    public Doctor(string name, string[] roles) : base(name)
    {
        Roles = roles;
    }
}
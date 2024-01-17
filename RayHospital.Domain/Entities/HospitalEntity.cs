namespace RayHospital.Domain.Entities;

public class HospitalEntity
{
    public HospitalEntity(string name)
    {
        Name = name;
    }
    public string Name { get; set; }
}
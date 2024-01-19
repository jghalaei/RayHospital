using System;
using RayHospital.Domain.Entities;

namespace RayHospital.App.Managers;

public interface IDoctorsManager
{
    public Doctor GetAvailableDoctors(DateTime date, EConditionType condition);
}

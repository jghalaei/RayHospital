using System;
using RayHospital.Domain.Entities;

namespace RayHospital.Interfaces;

public interface IRoomsManager
{
    public TreatmentRoom GetAvailableRoom(DateTime date, EConditionType condition, ETopography? topography);
}

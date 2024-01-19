using System;
using System.Linq;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Managers;
public class RoomsManager : IRoomsManager
{
    private readonly IRepository<TreatmentRoom> _roomRepository;
    private readonly IRepository<Consultation> _consultationRepository;

    public RoomsManager(IRepository<TreatmentRoom> roomRepository, IRepository<Consultation> consultationRepository)
    {
        _roomRepository = roomRepository;
        _consultationRepository = consultationRepository;
    }

    public TreatmentRoom GetAvailableRoom(DateTime date, EConditionType condition, ETopography? topography)
    {
        foreach (var room in _roomRepository.GetAll(x => IsRoomMatched(x, condition, topography)))
        {
            if (CheckRoomAvailability(date, room))
                return room;
        }
        return null;
    }

    private bool CheckRoomAvailability(DateTime date, TreatmentRoom room)
    {
        var book = _consultationRepository.GetOne(c => c.ConsultaionDate == date && c.RoomName == room.Name);
        return book == null;
    }


    private bool IsRoomMatched(TreatmentRoom room, EConditionType condition, ETopography? topography)
    {
        if (condition == EConditionType.Flu)
            return true;
        if (condition == EConditionType.Cancer && room.Machine is not null)
        {
            if (topography == ETopography.Breast)
                return true;
            if (topography == ETopography.HeadNeck && room.Machine.Capability == EMachineCapability.Advanced)
                return true;
        }
        return false;
    }

}
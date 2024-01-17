using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHospital.App.Services
{
    public class TreatmentRoomServices
    {
        private readonly IRepository<TreatmentRoom> _roomRepository;
        private readonly IRepository<Consultation> _consultationRepository;

        public TreatmentRoomServices(IRepository<TreatmentRoom> roomRepository, IRepository<Consultation> consultationRepository)
        {
            _roomRepository = roomRepository;
            _consultationRepository = consultationRepository;
        }

        public TreatmentRoom GetAvailableRoom(DateTime date, EConditionType condition, ETopography? topography)
        {
            foreach (var room in _roomRepository.GetAll().Where(x => IsRoomMatched(x, condition, topography)))
            {
                if (CheckRoomAvailability(date, room))
                    return room;
            }
            return null;
        }

        private bool CheckRoomAvailability(DateTime date, TreatmentRoom room)
        {
            var lastbooked = _consultationRepository.GetAll().LastOrDefault(x => x.RoomName == room.Name);
            return lastbooked == null || lastbooked.ConsultaionDate < date;
        }


        private bool IsRoomMatched(TreatmentRoom room, EConditionType condition, ETopography? topography)
        {
            if (condition == EConditionType.Flu)
                return true;
            if (condition == EConditionType.Cancer)
            {
                if (room.Machine is null)
                    return false;
                if (topography == ETopography.Breast)
                    return true;
                if (topography == ETopography.HeadNeck && room.Machine.Capability == EMachineCapability.Advanced)
                    return true;
            }
            return false;
        }

    }
}
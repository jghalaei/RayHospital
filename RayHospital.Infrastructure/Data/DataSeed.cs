using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using RayHospital.Domain.Entities;
using RayHospital.Resources;

namespace RayHospital.Infrastructure.Data
{
    public class DataSeeder
    {
        private readonly InMemoryData _inMemoryData;

        public DataSeeder(InMemoryData inMemoryData)
        {
            _inMemoryData = inMemoryData;
        }

        public void SeedData()
        {
            SeedDoctors();
            SeddMachines();
            SeedRooms();
        }

        private void SeedRooms()
        {
            foreach (var item in HospitalResources.TreatmentRooms)
            {
                var machine = _inMemoryData.GetAll<TreatmentMachine>().FirstOrDefault(m => m.Name == item.MachineName);
                TreatmentRoom room = new TreatmentRoom(item.Name, machine);
                _inMemoryData.Insert(room);
            }
        }

        private void SeddMachines()
        {
            foreach (var item in HospitalResources.TreatmentMachines)
            {
                TreatmentMachine machine = new TreatmentMachine(item.Name, Enum.Parse<EMachineCapability>(item.Capability));
                _inMemoryData.Insert(machine);
            }
        }

        private void SeedDoctors()
        {
            foreach (var item in HospitalResources.Doctors)
            {
                Doctor doctor = new Doctor(item.Name, item.Roles);
                _inMemoryData.Insert(doctor);
            }
        }
    }
}
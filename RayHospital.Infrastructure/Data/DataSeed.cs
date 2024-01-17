using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;
using RayHospital.Domain.Entities;
using RayHospital.Resources;

namespace RayHospital.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void SeedData()
        {
            foreach (var item in HospitalResources.Doctors)
            {
                Doctor doctor = new Doctor(item.Name, item.Roles);
                InMemoryData.GetInstance().Insert(doctor);
            }
            foreach (var item in HospitalResources.TreatmentMachines)
            {
                TreatmentMachine machine = new TreatmentMachine(item.Name, Enum.Parse<EMachineCapability>(item.Capability));
                InMemoryData.GetInstance().Insert(machine);
            }
            foreach (var item in HospitalResources.TreatmentRooms)
            {
                var machine = InMemoryData.GetInstance().GetAll<TreatmentMachine>().FirstOrDefault(m => m.Name == item.MachineName);
                TreatmentRoom room = new TreatmentRoom(item.Name, machine);
                InMemoryData.GetInstance().Insert(room);
            }
        }

    }
}
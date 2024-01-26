using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using RayHospital.App.Managers;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Infrastructure.Data;
using RayHospital.Infrastructure.Repositories;
using RayHospital.Interfaces;

namespace RayHosptial.App.Tests.Managers
{
    public class DoctorsManagerTests
    {
        [Fact]
        public void GetAvailableDoctor_ReturnsAvailableRoom()
        {

            // Arrange
            var mockDoctorRepository = new Mock<IRepository<Doctor>>();
            mockDoctorRepository.Setup(x => x.GetAll(It.IsAny<Func<Doctor, bool>>())).Returns(new List<Doctor> { new Doctor("Doctor1", ["GeneralPractitioner"]) });

            var mockConsultationRepository = new Mock<IRepository<Consultation>>();
            mockConsultationRepository.Setup(x => x.GetOne(It.IsAny<Func<Consultation, bool>>())).Returns(() => null);
            var doctorsManager = new DoctorsManager(mockDoctorRepository.Object, mockConsultationRepository.Object);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;
            // Act
            var result = doctorsManager.GetAvailableDoctors(date, condition);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Doctor1");
        }

        [Fact]
        public void GetAvailableDoctor_CurrentDay_NoAvailableDoctor()
        {

            // Arrange
            var mockDoctorRepository = new Mock<IRepository<Doctor>>();

            mockDoctorRepository.Setup(x => x.GetAll(It.IsAny<Func<Doctor, bool>>())).Returns(new List<Doctor> { new Doctor("Doctor1", ["GeneralPractitioner"]) });

            var mockConsultationRepository = new Mock<IRepository<Consultation>>();
            mockConsultationRepository.Setup(x => x.GetOne(It.IsAny<Func<Consultation, bool>>()))
                                      .Returns(() => new Consultation(DateTime.Now, "Patient1", "Doctor1", "Room1", DateTime.Now));
            var doctorsManager = new DoctorsManager(mockDoctorRepository.Object, mockConsultationRepository.Object);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;

            // Act
            var result = doctorsManager.GetAvailableDoctors(date, condition);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAvailableDoctor_Flu_Returns_GeneralParactioner()
        {

            // Arrange
            InMemoryData data = new InMemoryData();

            data.InsertMany(new List<Doctor>{
                new Doctor("Doctor1",["GeneralPractitioner"]),
                new Doctor("Doctor2",["Oncologist"]),
                new Doctor("Doctor3",["Oncologist","GeneralPractitioner"])
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor1", "Room1", DateTime.Now)
            });

            var doctorRepository = new Repository<Doctor>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new DoctorsManager(doctorRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;

            // Act
            var result = doctorsManager.GetAvailableDoctors(date, condition);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Doctor3", result.Name);
        }
        [Fact]
        public void GetAvailableDoctor_Flu_Returns_NoDoctor()
        {

            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<Doctor>{
                new Doctor("Doctor1",["GeneralPractitioner"]),
                new Doctor("Doctor2",["Oncologist"]),
                new Doctor("Doctor3",["Oncologist","GeneralPractitioner"])
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor1", "Room1", DateTime.Now),
                new Consultation(DateTime.Now, "Patient2", "Doctor3", "Room2", DateTime.Now)
            });

            var doctorRepository = new Repository<Doctor>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new DoctorsManager(doctorRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;

            // Act
            var result = doctorsManager.GetAvailableDoctors(date, condition);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void GetAvailableDoctor_Cancer_Returns_Oncologist()
        {

            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<Doctor>{
                new Doctor("Doctor1",["GeneralPractitioner"]),
                new Doctor("Doctor2",["Oncologist"]),
                new Doctor("Doctor3",["Oncologist","GeneralPractitioner"])
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room1", DateTime.Now),
            });

            var doctorRepository = new Repository<Doctor>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new DoctorsManager(doctorRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;

            // Act
            var result = doctorsManager.GetAvailableDoctors(date, condition);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Doctor3", result.Name);
        }

        [Fact]
        public void GetAvailableDoctor_Cancer_Returns_NoDoctor()
        {

            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<Doctor>{
                new Doctor("Doctor1",["GeneralPractitioner"]),
                new Doctor("Doctor2",["Oncologist"]),
                new Doctor("Doctor3",["Oncologist","GeneralPractitioner"])
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room1", DateTime.Now),
                new Consultation(DateTime.Now, "Patient2", "Doctor3", "Room2", DateTime.Now)
            });

            var doctorRepository = new Repository<Doctor>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new DoctorsManager(doctorRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;

            // Act
            var result = doctorsManager.GetAvailableDoctors(date, condition);

            // Assert
            Assert.Null(result);
        }
    }
}
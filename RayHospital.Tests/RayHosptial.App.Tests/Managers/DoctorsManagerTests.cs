using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using RayHospital.App.Managers;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;

namespace RayHosptial.App.Tests.Managers
{
    public class DoctorsManagerTests
    {
        [Fact]
        public void GetAvailableRoom_ReturnsAvailableRoom()
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
        //TODO: Add more tests for differenct scenarios
    }
}
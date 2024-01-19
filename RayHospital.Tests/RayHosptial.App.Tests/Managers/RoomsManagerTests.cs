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
    public class RoomsManagerTests
    {
        [Fact]
        public void GetAvailableRoom_ReturnsAvailableRoom()
        {

            // Arrange
            var mockRoomRepository = new Mock<IRepository<TreatmentRoom>>();
            mockRoomRepository.Setup(x => x.GetAll(It.IsAny<Func<TreatmentRoom, bool>>())).Returns(new List<TreatmentRoom> { new TreatmentRoom("Room1") });

            var mockConsultationRepository = new Mock<IRepository<Consultation>>();
            mockConsultationRepository.Setup(x => x.GetOne(It.IsAny<Func<Consultation, bool>>())).Returns(() => null);
            var roomsManager = new RoomsManager(mockRoomRepository.Object, mockConsultationRepository.Object);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.HeadNeck;

            // Act
            var result = roomsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Room1");

        }
        //TODO: Add more tests for differenct scenarios
    }
}
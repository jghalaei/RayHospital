using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NuGet.Frameworks;
using RayHospital.App.Managers;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Infrastructure.Data;
using RayHospital.Infrastructure.Repositories;
using RayHospital.Interfaces;

namespace RayHosptial.App.Tests.Managers
{
    public class RoomsManagerTests
    {
        [Fact]
        public void GetAvailableRoom_ReturnsAvailableRoom_CurrentDay()
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

        [Fact]
        public void GetAvailableRoom_CurrentDay_NoAvailableRoom()
        {
            // Arrange
            var mockRoomRepository = new Mock<IRepository<TreatmentRoom>>();
            mockRoomRepository.Setup(x => x.GetAll(It.IsAny<Func<TreatmentRoom, bool>>())).Returns(new List<TreatmentRoom> { new TreatmentRoom("Room1") });

            var mockConsultationRepository = new Mock<IRepository<Consultation>>();
            Consultation previousBook = new(DateTime.Now, "Patient1", "Doctor1", "Room1", DateTime.Now);
            mockConsultationRepository.Setup(x => x.GetOne(It.IsAny<Func<Consultation, bool>>())).Returns(() => previousBook);

            var roomsManager = new RoomsManager(mockRoomRepository.Object, mockConsultationRepository.Object);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.HeadNeck;

            // Act
            var result = roomsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void GetAvailableRoom_CurrentDay_NoRoom()
        {
            // Arrange
            var mockRoomRepository = new Mock<IRepository<TreatmentRoom>>();
            mockRoomRepository.Setup(x => x.GetAll(It.IsAny<Func<TreatmentRoom, bool>>())).Returns(() => new List<TreatmentRoom>());

            var mockConsultationRepository = new Mock<IRepository<Consultation>>();
            mockConsultationRepository.Setup(x => x.GetOne(It.IsAny<Func<Consultation, bool>>())).Returns(() => null);

            var roomsManager = new RoomsManager(mockRoomRepository.Object, mockConsultationRepository.Object);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.HeadNeck;

            // Act
            var result = roomsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void GetAvailableRoom_Flu_Returns_Room()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3")
            });
            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Room1", result.Name);
        }
        [Fact]
        public void GetAvailableRoom_Flu_Returns_FirstAvailableRoom()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3")
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room1", DateTime.Now),
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room2", DateTime.Now),
            });

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Room3", result.Name);
        }
        [Fact]
        public void GetAvailableRoom_Flu_NotAvailable()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3")
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room1", DateTime.Now),
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room2", DateTime.Now),
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room3", DateTime.Now),
            });

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Flu;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAvailableRoom_Cancer_HeadNeck_Return_AdvancedRoom()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3",new TreatmentMachine("Machine3",EMachineCapability.Advanced))
            });

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.HeadNeck;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Room3", result.Name);
        }
        [Fact]
        public void GetAvailableRoom_Cancer_HeadNeck_Returns_null()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3",new TreatmentMachine("Machine3",EMachineCapability.Advanced))
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room3", DateTime.Now)
            });

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.HeadNeck;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.Null(result);
        }
        [Fact]
        public void GetAvailableRoom_Cancer_Breast_Return_SimpleRoom()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3",new TreatmentMachine("Machine3",EMachineCapability.Advanced))
            });

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.Breast;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Room2", result.Name);
        }
        [Fact]
        public void GetAvailableRoom_Cancer_Breast_Return_AdvancedRoom()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3",new TreatmentMachine("Machine3",EMachineCapability.Advanced))
            });
            data.Insert(new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room2", DateTime.Now));

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.Breast;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Room3", result.Name);
        }
        [Fact]
        public void GetAvailableRoom_Cancer_Breast_Returns_null()
        {
            // Arrange
            InMemoryData data = new InMemoryData();
            data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3",new TreatmentMachine("Machine3",EMachineCapability.Advanced))
            });
            data.InsertMany(new List<Consultation>{
                new Consultation(DateTime.Now, "Patient1", "Doctor2", "Room2", DateTime.Now),
                new Consultation(DateTime.Now, "Patient2", "Doctor3", "Room3", DateTime.Now)
            });

            var roomRepository = new Repository<TreatmentRoom>(data);
            var consultationRepository = new Repository<Consultation>(data);

            var doctorsManager = new RoomsManager(roomRepository, consultationRepository);
            var date = DateTime.Now;
            var condition = EConditionType.Cancer;
            var topography = ETopography.Breast;
            // Act
            var result = doctorsManager.GetAvailableRoom(date, condition, topography);

            // Assert
            Assert.Null(result);
        }
    }
}
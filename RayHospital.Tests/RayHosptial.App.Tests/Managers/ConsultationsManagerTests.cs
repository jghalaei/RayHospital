using Moq;
using RayHospital.App.Managers;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;
using RayHospital.App.Mappers;
using System.Runtime.ConstrainedExecution;
using Castle.Core.Resource;
using RayHospital.Infrastructure.Data;
using System.Runtime.Versioning;
using RayHospital.Infrastructure.Repositories;

namespace RayHosptial.App.Tests.Managers
{
    public class InMemoryDataFixture : IDisposable
    {
        public InMemoryData Data { get; private set; }
        public InMemoryDataFixture()
        {
            Data = new InMemoryData();
            Data.InsertMany(new List<Doctor>(){
                new Doctor("Doctor1", ["GeneralPractitioner"]),
                new Doctor("Doctor2", ["Oncologist"]),
                new Doctor("Doctor3", ["GeneralPractitioner","Oncologist"])
            });
            Data.InsertMany(new List<TreatmentRoom>{
                new TreatmentRoom("Room1"),
                new TreatmentRoom("Room2",new TreatmentMachine("Machine2",EMachineCapability.Simple)),
                new TreatmentRoom("Room3",new TreatmentMachine("Machine3",EMachineCapability.Advanced))
            });
        }

        public void Dispose()
        {

        }
    }
    public class ConsultationsManagerTests : IClassFixture<InMemoryDataFixture>
    {
        private readonly InMemoryData _inMemoryData;
        public ConsultationsManagerTests(InMemoryDataFixture fixture)
        {
            _inMemoryData = fixture.Data;
        }
        [Fact]
        public void BookConsultation_Should_Insert_Consultation()
        {
            // Arrange
            var registrationDate = DateTime.Today;
            var patient = new PatientRegistrationModel { Name = "Patient1", Condition = "Flu", Topography = "" };
            var consultation = new Consultation(registrationDate, patient.Name, "Doctor1", "Room1", registrationDate);

            var consultationRepositoryMock = new Mock<IRepository<Consultation>>();
            consultationRepositoryMock.Setup(x => x.Insert(It.IsAny<Consultation>()));
            consultationRepositoryMock.Setup(x => x.GetAll(It.IsAny<Func<Consultation, bool>>())).Returns(new List<Consultation> { consultation });

            var roomServicesMock = new Mock<IRoomsManager>();
            roomServicesMock.Setup(x => x.GetAvailableRoom(It.IsAny<DateTime>(), It.IsAny<EConditionType>(), It.IsAny<ETopography?>()))
                    .Returns(new TreatmentRoom("Room1"));

            var doctorServicesMock = new Mock<IDoctorsManager>();
            doctorServicesMock.Setup(x => x.GetAvailableDoctors(It.IsAny<DateTime>(), It.IsAny<EConditionType>()))
                .Returns(new Doctor("Doctor1", ["GeneralPractitioner"]));

            var consultationsManager = new ConsultationsManager(consultationRepositoryMock.Object, roomServicesMock.Object, doctorServicesMock.Object);

            // Act
            var result = consultationsManager.BookConsultation(registrationDate, patient);

            // Assert
            consultationRepositoryMock.Verify(x => x.Insert(It.IsAny<Consultation>()), Times.Once);
            Assert.Equivalent(consultation.ToConsultationModel(), result);
        }

        public static IEnumerable<object[]> TestData()
        {
            var testData = new List<object[]>(){
                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Flu", Topography = ""},
                    new ConsultationModel { RoomName = "Room1", DoctorName = "Doctor1", Date = DateTime.Today, PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Cancer", Topography = "HeadNeck"},
                    new ConsultationModel { RoomName = "Room3", DoctorName = "Doctor2", Date = DateTime.Today, PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Cancer", Topography = "Breast"},
                    new ConsultationModel { RoomName = "Room2", DoctorName = "Doctor3", Date = DateTime.Today, PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Flu", Topography = ""},
                    new ConsultationModel { RoomName = "Room1", DoctorName = "Doctor1", Date = DateTime.Today.AddDays(1), PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Flu", Topography = ""},
                    new ConsultationModel { RoomName = "Room2", DoctorName = "Doctor3", Date = DateTime.Today.AddDays(1), PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Cancer", Topography = "Breast"},
                    new ConsultationModel { RoomName = "Room3", DoctorName = "Doctor2", Date = DateTime.Today.AddDays(1), PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Cancer", Topography = "HeadNeck"},
                    new ConsultationModel { RoomName = "Room3", DoctorName = "Doctor2", Date = DateTime.Today.AddDays(2), PatientName = "Patient1"} },

                new object[] {0, new PatientRegistrationModel { Name = "Patient1", Condition = "Cancer", Topography = "HeadNeck"},
                    new ConsultationModel { RoomName = "Room3", DoctorName = "Doctor2", Date = DateTime.Today.AddDays(3), PatientName = "Patient1"} }
            };

            return testData;
        }
        [Theory]
        [MemberData(nameof(TestData))]
        public void BookConsultation(int day, PatientRegistrationModel registration, ConsultationModel expectedResult)
        {
            // Arrange

            var roomRepository = new Repository<TreatmentRoom>(_inMemoryData);
            var doctorRepository = new Repository<Doctor>(_inMemoryData);
            var consultationRepository = new Repository<Consultation>(_inMemoryData);

            var roomsManager = new RoomsManager(roomRepository, consultationRepository);
            var doctorsManager = new DoctorsManager(doctorRepository, consultationRepository);
            var consultationsManager = new ConsultationsManager(consultationRepository, roomsManager, doctorsManager);
            //Act
            var result = consultationsManager.BookConsultation(DateTime.Today.AddDays(day), registration);
            // Assert
            Assert.Equivalent(expectedResult, result);
        }
    }
}
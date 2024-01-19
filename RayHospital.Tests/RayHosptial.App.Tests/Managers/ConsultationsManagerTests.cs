using Moq;
using RayHospital.App.Managers;
using RayHospital.Domain.Entities;
using RayHospital.Domain.Entities.Consultation;
using RayHospital.Interfaces;
using RayHospital.App.Mappers;
using System.Runtime.ConstrainedExecution;
using Castle.Core.Resource;
using RayHospital.Infrastructure.Data;

namespace RayHosptial.App.Tests.Managers
{
    public class ConsultationsManagerTests
    {
        [Fact]
        public void BookConsultation_Should_Insert_Consultation()
        {
            // Arrange
            var registrationDate = DateTime.Now;
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
        //TODO: ADD TESTS For differenct scenarios
    }
}
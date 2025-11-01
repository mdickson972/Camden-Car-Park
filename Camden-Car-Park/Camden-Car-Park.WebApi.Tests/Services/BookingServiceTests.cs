using Camden_Car_Park.Common.Models.Enums;
using Camden_Car_Park.Common.Models.ReadModels;
using Camden_Car_Park.WebApi.Data.Tables;
using Camden_Car_Park.WebApi.Repositories;
using Camden_Car_Park.WebApi.Services;
using Moq;

namespace Camden_Car_Park.WebApi.Tests.Services
{
    [TestFixture]
    public class BookingServiceTests
    {
        private Mock<IBookingRepository> _mockBookingRepository;
        private Mock<IEmployeeRepository> _mockEmployeeRepository;
        private BookingService _bookingService;

        [SetUp]
        public void Setup()
        {
            _mockBookingRepository = new Mock<IBookingRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _bookingService = new BookingService(_mockBookingRepository.Object, _mockEmployeeRepository.Object);
        }

        #region GetAllBookingsAsync Tests

        [Test]
        public async Task GetAllBookingsAsync_ReturnsEmptyList_WhenNoBookingsExist()
        {
            // Arrange
            _mockBookingRepository
                .Setup(repo => repo.GetAllBookingsAsync())
                .ReturnsAsync(new List<Booking>());

            // Act
            var result = await _bookingService.GetAllBookingsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
            _mockBookingRepository.Verify(repo => repo.GetAllBookingsAsync(), Times.Once);
        }

        [Test]
        public async Task GetAllBookingsAsync_ReturnsBookingResponses_WhenBookingsExist()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, Name = "John Doe" };
            var bookings = new List<Booking>
            {
                new Booking
                {
                    BookingId = 1,
                    Employee = employee,
                    RegistrationNumber = "ABC123",
                    Make = "Toyota",
                    Model = "Camry",
                    Colour = "Blue",
                    Year = "2022",
                    ApprovalStatus = ApprovalStatus.Approved,
                    ApprovalDate = new DateTime(2024, 1, 15)
                },
                new Booking
                {
                    BookingId = 2,
                    Employee = employee,
                    RegistrationNumber = "XYZ789",
                    Make = "Honda",
                    Model = "Civic",
                    Colour = "Red",
                    Year = "2023",
                    ApprovalStatus = ApprovalStatus.Pending,
                    ApprovalDate = default
                }
            };

            _mockBookingRepository
                .Setup(repo => repo.GetAllBookingsAsync())
                .ReturnsAsync(bookings);

            // Act
            var result = await _bookingService.GetAllBookingsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            var bookingList = result.ToList();
            Assert.That(bookingList, Has.Count.EqualTo(2));
            
            Assert.That(bookingList[0].BookingId, Is.EqualTo(1));
            Assert.That(bookingList[0].EmployeeId, Is.EqualTo(1));
            Assert.That(bookingList[0].EmployeeName, Is.EqualTo("John Doe"));
            Assert.That(bookingList[0].VehicleRegistrationNumber, Is.EqualTo("ABC123"));
            Assert.That(bookingList[0].VehicleMake, Is.EqualTo("Toyota"));
            Assert.That(bookingList[0].VehicleModel, Is.EqualTo("Camry"));
            Assert.That(bookingList[0].VehicleColour, Is.EqualTo("Blue"));
            Assert.That(bookingList[0].VehicleYear, Is.EqualTo("2022"));
            Assert.That(bookingList[0].ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));

            _mockBookingRepository.Verify(repo => repo.GetAllBookingsAsync(), Times.Once);
        }

        #endregion

        #region GetBookingAsync Tests

        [Test]
        public async Task GetBookingAsync_ReturnsNull_WhenBookingDoesNotExist()
        {
            // Arrange
            _mockBookingRepository
                .Setup(repo => repo.GetBookingAsync(It.IsAny<int>()))
                .ReturnsAsync((Booking)null);

            // Act
            var result = await _bookingService.GetBookingAsync(999);

            // Assert
            Assert.That(result, Is.Null);
            _mockBookingRepository.Verify(repo => repo.GetBookingAsync(999), Times.Once);
        }

        [Test]
        public async Task GetBookingAsync_ReturnsBookingResponse_WhenBookingExists()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, Name = "Jane Smith" };
            var booking = new Booking
            {
                BookingId = 1,
                Employee = employee,
                RegistrationNumber = "DEF456",
                Make = "Ford",
                Model = "Focus",
                Colour = "Green",
                Year = "2021",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = new DateTime(2024, 2, 20)
            };

            _mockBookingRepository
                .Setup(repo => repo.GetBookingAsync(1))
                .ReturnsAsync(booking);

            // Act
            var result = await _bookingService.GetBookingAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BookingId, Is.EqualTo(1));
            Assert.That(result.EmployeeId, Is.EqualTo(1));
            Assert.That(result.EmployeeName, Is.EqualTo("Jane Smith"));
            Assert.That(result.VehicleRegistrationNumber, Is.EqualTo("DEF456"));
            Assert.That(result.VehicleMake, Is.EqualTo("Ford"));
            Assert.That(result.VehicleModel, Is.EqualTo("Focus"));
            Assert.That(result.VehicleColour, Is.EqualTo("Green"));
            Assert.That(result.VehicleYear, Is.EqualTo("2021"));
            Assert.That(result.ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));
            Assert.That(result.ApprovalDate, Is.EqualTo(new DateTime(2024, 2, 20)));

            _mockBookingRepository.Verify(repo => repo.GetBookingAsync(1), Times.Once);
        }

        #endregion

        #region CreateBookingAsync Tests

        [Test]
        public async Task CreateBookingAsync_CallsRepositories_WithCorrectData()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, Name = "Test Employee" };
            var bookingRequest = new BookingRequest
            {
                BookingId = 0,
                EmployeeId = 1,
                VehicleRegistrationNumber = "GHI789",
                VehicleMake = "Nissan",
                VehicleModel = "Altima",
                VehicleColour = "Black",
                VehicleYear = "2023",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = DateTime.Now
            };

            _mockEmployeeRepository
                .Setup(repo => repo.GetEmployeeAsync(1))
                .ReturnsAsync(employee);

            _mockBookingRepository
                .Setup(repo => repo.AddBookingAsync(It.IsAny<Booking>()))
                .Returns(Task.CompletedTask);

            // Act
            await _bookingService.CreateBookingAsync(bookingRequest);

            // Assert
            _mockEmployeeRepository.Verify(repo => repo.GetEmployeeAsync(1), Times.Once);
            _mockBookingRepository.Verify(repo => repo.AddBookingAsync(It.Is<Booking>(b =>
                b.Employee == employee &&
                b.RegistrationNumber == "GHI789" &&
                b.Make == "Nissan" &&
                b.Model == "Altima" &&
                b.Colour == "Black" &&
                b.Year == "2023" &&
                b.ApprovalStatus == ApprovalStatus.Pending
            )), Times.Once);
        }

        [Test]
        public async Task CreateBookingAsync_MapsAllProperties_Correctly()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 2, Name = "Another Employee" };
            var approvalDate = new DateTime(2024, 3, 15);
            var bookingRequest = new BookingRequest
            {
                BookingId = 0,
                EmployeeId = 2,
                VehicleRegistrationNumber = "JKL012",
                VehicleMake = "BMW",
                VehicleModel = "X5",
                VehicleColour = "White",
                VehicleYear = "2024",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = approvalDate
            };

            _mockEmployeeRepository
                .Setup(repo => repo.GetEmployeeAsync(2))
                .ReturnsAsync(employee);

            Booking capturedBooking = null;
            _mockBookingRepository
                .Setup(repo => repo.AddBookingAsync(It.IsAny<Booking>()))
                .Callback<Booking>(b => capturedBooking = b)
                .Returns(Task.CompletedTask);

            // Act
            await _bookingService.CreateBookingAsync(bookingRequest);

            // Assert
            Assert.That(capturedBooking, Is.Not.Null);
            Assert.That(capturedBooking.Employee, Is.EqualTo(employee));
            Assert.That(capturedBooking.RegistrationNumber, Is.EqualTo("JKL012"));
            Assert.That(capturedBooking.Make, Is.EqualTo("BMW"));
            Assert.That(capturedBooking.Model, Is.EqualTo("X5"));
            Assert.That(capturedBooking.Colour, Is.EqualTo("White"));
            Assert.That(capturedBooking.Year, Is.EqualTo("2024"));
            Assert.That(capturedBooking.ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));
            Assert.That(capturedBooking.ApprovalDate, Is.EqualTo(approvalDate));
        }

        #endregion

        #region UpdateBookingAsync Tests

        [Test]
        public async Task UpdateBookingAsync_CallsRepositories_WithCorrectData()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 1, Name = "Updated Employee" };
            var bookingRequest = new BookingRequest
            {
                BookingId = 5,
                EmployeeId = 1,
                VehicleRegistrationNumber = "MNO345",
                VehicleMake = "Audi",
                VehicleModel = "A4",
                VehicleColour = "Silver",
                VehicleYear = "2022",
                ApprovalStatus = ApprovalStatus.Cancelled,
                ApprovalDate = DateTime.Now
            };

            _mockEmployeeRepository
                .Setup(repo => repo.GetEmployeeAsync(1))
                .ReturnsAsync(employee);

            _mockBookingRepository
                .Setup(repo => repo.UpdateBookingAsync(It.IsAny<Booking>()))
                .Returns(Task.CompletedTask);

            // Act
            await _bookingService.UpdateBookingAsync(bookingRequest);

            // Assert
            _mockEmployeeRepository.Verify(repo => repo.GetEmployeeAsync(1), Times.Once);
            _mockBookingRepository.Verify(repo => repo.UpdateBookingAsync(It.Is<Booking>(b =>
                b.BookingId == 5 &&
                b.Employee == employee &&
                b.RegistrationNumber == "MNO345" &&
                b.Make == "Audi" &&
                b.Model == "A4" &&
                b.Colour == "Silver" &&
                b.Year == "2022" &&
                b.ApprovalStatus == ApprovalStatus.Cancelled
            )), Times.Once);
        }

        [Test]
        public async Task UpdateBookingAsync_MapsAllProperties_IncludingBookingId()
        {
            // Arrange
            var employee = new Employee { EmployeeId = 3, Name = "Test User" };
            var approvalDate = new DateTime(2024, 4, 10);
            var bookingRequest = new BookingRequest
            {
                BookingId = 10,
                EmployeeId = 3,
                VehicleRegistrationNumber = "PQR678",
                VehicleMake = "Mercedes",
                VehicleModel = "C-Class",
                VehicleColour = "Gray",
                VehicleYear = "2023",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = approvalDate
            };

            _mockEmployeeRepository
                .Setup(repo => repo.GetEmployeeAsync(3))
                .ReturnsAsync(employee);

            Booking capturedBooking = null;
            _mockBookingRepository
                .Setup(repo => repo.UpdateBookingAsync(It.IsAny<Booking>()))
                .Callback<Booking>(b => capturedBooking = b)
                .Returns(Task.CompletedTask);

            // Act
            await _bookingService.UpdateBookingAsync(bookingRequest);

            // Assert
            Assert.That(capturedBooking, Is.Not.Null);
            Assert.That(capturedBooking.BookingId, Is.EqualTo(10));
            Assert.That(capturedBooking.Employee, Is.EqualTo(employee));
            Assert.That(capturedBooking.RegistrationNumber, Is.EqualTo("PQR678"));
            Assert.That(capturedBooking.Make, Is.EqualTo("Mercedes"));
            Assert.That(capturedBooking.Model, Is.EqualTo("C-Class"));
            Assert.That(capturedBooking.Colour, Is.EqualTo("Gray"));
            Assert.That(capturedBooking.Year, Is.EqualTo("2023"));
            Assert.That(capturedBooking.ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));
            Assert.That(capturedBooking.ApprovalDate, Is.EqualTo(approvalDate));
        }

        #endregion
    }
}

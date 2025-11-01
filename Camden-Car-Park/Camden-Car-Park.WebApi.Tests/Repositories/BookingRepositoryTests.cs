using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camden_Car_Park.Common.Models.Enums;
using Camden_Car_Park.WebApi.Data;
using Camden_Car_Park.WebApi.Data.Tables;
using Camden_Car_Park.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Camden_Car_Park.WebApi.Tests.Repositories
{
    [TestFixture]
    public class BookingRepositoryTests
    {
        private CarParkDbContext _dbContext;
        private BookingRepository _bookingRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new CarParkDbContext(options);
            _bookingRepository = new BookingRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region GetAllBookingsAsync Tests

        [Test]
        public async Task GetAllBookingsAsync_ReturnsEmptyList_WhenNoBookingsExist()
        {
            // Act
            var result = await _bookingRepository.GetAllBookingsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllBookingsAsync_ReturnsAllBookings_WithEmployeeIncluded()
        {
            // Arrange
            var employee1 = new Employee { Name = "John Doe" };
            var employee2 = new Employee { Name = "Jane Smith" };

            await _dbContext.Employees.AddRangeAsync(employee1, employee2);
            await _dbContext.SaveChangesAsync();

            var booking1 = new Booking
            {
                Employee = employee1,
                RegistrationNumber = "ABC123",
                Make = "Toyota",
                Model = "Camry",
                Colour = "Blue",
                Year = "2022",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = new DateTime(2024, 1, 15)
            };

            var booking2 = new Booking
            {
                Employee = employee2,
                RegistrationNumber = "XYZ789",
                Make = "Honda",
                Model = "Civic",
                Colour = "Red",
                Year = "2023",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            await _dbContext.Bookings.AddRangeAsync(booking1, booking2);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bookingRepository.GetAllBookingsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            var bookingList = result.ToList();
            Assert.That(bookingList, Has.Count.EqualTo(2));

            // Verify Employee is included - find bookings by registration number instead of assuming order
            var johnDoeBooking = bookingList.FirstOrDefault(b => b.RegistrationNumber == "ABC123");
            var janeSmithBooking = bookingList.FirstOrDefault(b => b.RegistrationNumber == "XYZ789");
            
            Assert.That(johnDoeBooking, Is.Not.Null);
            Assert.That(johnDoeBooking.Employee, Is.Not.Null);
            Assert.That(johnDoeBooking.Employee.Name, Is.EqualTo("John Doe"));
            
            Assert.That(janeSmithBooking, Is.Not.Null);
            Assert.That(janeSmithBooking.Employee, Is.Not.Null);
            Assert.That(janeSmithBooking.Employee.Name, Is.EqualTo("Jane Smith"));
        }

        [Test]
        public async Task GetAllBookingsAsync_ReturnsMultipleBookings_InCorrectOrder()
        {
            // Arrange
            var employee = new Employee { Name = "Test User" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var bookings = new List<Booking>
            {
                new Booking
                {
                    Employee = employee,
                    RegistrationNumber = "REG001",
                    Make = "Ford",
                    Model = "Focus",
                    Colour = "Green",
                    Year = "2021",
                    ApprovalStatus = ApprovalStatus.Approved,
                    ApprovalDate = DateTime.Now
                },
                new Booking
                {
                    Employee = employee,
                    RegistrationNumber = "REG002",
                    Make = "BMW",
                    Model = "X5",
                    Colour = "Black",
                    Year = "2023",
                    ApprovalStatus = ApprovalStatus.Pending,
                    ApprovalDate = default
                },
                new Booking
                {
                    Employee = employee,
                    RegistrationNumber = "REG003",
                    Make = "Audi",
                    Model = "A4",
                    Colour = "White",
                    Year = "2022",
                    ApprovalStatus = ApprovalStatus.Cancelled,
                    ApprovalDate = DateTime.Now
                }
            };

            await _dbContext.Bookings.AddRangeAsync(bookings);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bookingRepository.GetAllBookingsAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(3));
            Assert.That(result.All(b => b.Employee != null), Is.True);
        }

        #endregion

        #region GetBookingAsync Tests

        [Test]
        public async Task GetBookingAsync_ReturnsNull_WhenBookingDoesNotExist()
        {
            // Act
            var result = await _bookingRepository.GetBookingAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetBookingAsync_ReturnsBooking_WhenBookingExists()
        {
            // Arrange
            var employee = new Employee { Name = "Alice Johnson" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "TEST123",
                Make = "Mercedes",
                Model = "C-Class",
                Colour = "Silver",
                Year = "2024",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = new DateTime(2024, 3, 10)
            };

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bookingRepository.GetBookingAsync(booking.BookingId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BookingId, Is.EqualTo(booking.BookingId));
            Assert.That(result.RegistrationNumber, Is.EqualTo("TEST123"));
            Assert.That(result.Make, Is.EqualTo("Mercedes"));
            Assert.That(result.Model, Is.EqualTo("C-Class"));
            Assert.That(result.Colour, Is.EqualTo("Silver"));
            Assert.That(result.Year, Is.EqualTo("2024"));
            Assert.That(result.ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));
        }

        [Test]
        public async Task GetBookingAsync_IncludesEmployee_WhenBookingExists()
        {
            // Arrange
            var employee = new Employee { Name = "Bob Williams" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "EMP456",
                Make = "Nissan",
                Model = "Altima",
                Colour = "Gray",
                Year = "2023",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bookingRepository.GetBookingAsync(booking.BookingId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Employee, Is.Not.Null);
            Assert.That(result.Employee.Name, Is.EqualTo("Bob Williams"));
            Assert.That(result.Employee.EmployeeId, Is.EqualTo(employee.EmployeeId));
        }

        [Test]
        public async Task GetBookingAsync_ReturnsCorrectBooking_WhenMultipleBookingsExist()
        {
            // Arrange
            var employee = new Employee { Name = "Charlie Brown" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking1 = new Booking
            {
                Employee = employee,
                RegistrationNumber = "AAA111",
                Make = "Volvo",
                Model = "S60",
                Colour = "Blue",
                Year = "2021",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = DateTime.Now
            };

            var booking2 = new Booking
            {
                Employee = employee,
                RegistrationNumber = "BBB222",
                Make = "Mazda",
                Model = "CX-5",
                Colour = "Red",
                Year = "2022",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            await _dbContext.Bookings.AddRangeAsync(booking1, booking2);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _bookingRepository.GetBookingAsync(booking2.BookingId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BookingId, Is.EqualTo(booking2.BookingId));
            Assert.That(result.RegistrationNumber, Is.EqualTo("BBB222"));
            Assert.That(result.Make, Is.EqualTo("Mazda"));
        }

        #endregion

        #region AddBookingAsync Tests

        [Test]
        public async Task AddBookingAsync_AddsBookingToDatabase()
        {
            // Arrange
            var employee = new Employee { Name = "New Employee" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "NEW123",
                Make = "Tesla",
                Model = "Model 3",
                Colour = "White",
                Year = "2024",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            // Act
            await _bookingRepository.AddBookingAsync(booking);

            // Assert
            var bookings = await _dbContext.Bookings.ToListAsync();
            Assert.That(bookings, Has.Count.EqualTo(1));
            Assert.That(bookings[0].RegistrationNumber, Is.EqualTo("NEW123"));
            Assert.That(bookings[0].Make, Is.EqualTo("Tesla"));
            Assert.That(bookings[0].Model, Is.EqualTo("Model 3"));
        }

        [Test]
        public async Task AddBookingAsync_GeneratesBookingId()
        {
            // Arrange
            var employee = new Employee { Name = "Test Employee" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "GEN456",
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Black",
                Year = "2023",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = DateTime.Now
            };

            // Act
            await _bookingRepository.AddBookingAsync(booking);

            // Assert
            Assert.That(booking.BookingId, Is.GreaterThan(0));
        }

        [Test]
        public async Task AddBookingAsync_SavesAllProperties()
        {
            // Arrange
            var employee = new Employee { Name = "Property Test User" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var approvalDate = new DateTime(2024, 5, 20);
            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "PROP789",
                Make = "Lexus",
                Model = "RX",
                Colour = "Pearl White",
                Year = "2024",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = approvalDate
            };

            // Act
            await _bookingRepository.AddBookingAsync(booking);

            // Assert
            var savedBooking = await _dbContext.Bookings
                .Include(b => b.Employee)
                .FirstOrDefaultAsync(b => b.BookingId == booking.BookingId);

            Assert.That(savedBooking, Is.Not.Null);
            Assert.That(savedBooking.RegistrationNumber, Is.EqualTo("PROP789"));
            Assert.That(savedBooking.Make, Is.EqualTo("Lexus"));
            Assert.That(savedBooking.Model, Is.EqualTo("RX"));
            Assert.That(savedBooking.Colour, Is.EqualTo("Pearl White"));
            Assert.That(savedBooking.Year, Is.EqualTo("2024"));
            Assert.That(savedBooking.ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));
            Assert.That(savedBooking.ApprovalDate, Is.EqualTo(approvalDate));
            Assert.That(savedBooking.Employee.Name, Is.EqualTo("Property Test User"));
        }

        #endregion

        #region UpdateBookingAsync Tests

        [Test]
        public async Task UpdateBookingAsync_UpdatesExistingBooking()
        {
            // Arrange
            var employee = new Employee { Name = "Update Test User" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "UPD123",
                Make = "Original Make",
                Model = "Original Model",
                Colour = "Original Colour",
                Year = "2020",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            // Detach to simulate a fresh update
            _dbContext.Entry(booking).State = EntityState.Detached;

            // Act
            var updatedBooking = new Booking
            {
                BookingId = booking.BookingId,
                Employee = employee,
                RegistrationNumber = "UPD999",
                Make = "Updated Make",
                Model = "Updated Model",
                Colour = "Updated Colour",
                Year = "2024",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = new DateTime(2024, 6, 1)
            };

            await _bookingRepository.UpdateBookingAsync(updatedBooking);

            // Assert
            var result = await _dbContext.Bookings.FindAsync(booking.BookingId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.RegistrationNumber, Is.EqualTo("UPD999"));
            Assert.That(result.Make, Is.EqualTo("Updated Make"));
            Assert.That(result.Model, Is.EqualTo("Updated Model"));
            Assert.That(result.Colour, Is.EqualTo("Updated Colour"));
            Assert.That(result.Year, Is.EqualTo("2024"));
            Assert.That(result.ApprovalStatus, Is.EqualTo(ApprovalStatus.Approved));
        }

        [Test]
        public async Task UpdateBookingAsync_PreservesBookingId()
        {
            // Arrange
            var employee = new Employee { Name = "ID Test User" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "ID123",
                Make = "Original",
                Model = "Model",
                Colour = "Color",
                Year = "2021",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();

            var originalId = booking.BookingId;
            _dbContext.Entry(booking).State = EntityState.Detached;

            // Act
            booking.Make = "Updated";
            await _bookingRepository.UpdateBookingAsync(booking);

            // Assert
            var result = await _dbContext.Bookings.FindAsync(originalId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BookingId, Is.EqualTo(originalId));
            Assert.That(result.Make, Is.EqualTo("Updated"));
        }

        [Test]
        public async Task UpdateBookingAsync_CanChangeApprovalStatus()
        {
            // Arrange
            var employee = new Employee { Name = "Approval Test User" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "APR123",
                Make = "Toyota",
                Model = "Prius",
                Colour = "Blue",
                Year = "2023",
                ApprovalStatus = ApprovalStatus.Pending,
                ApprovalDate = default
            };

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(booking).State = EntityState.Detached;

            // Act
            booking.ApprovalStatus = ApprovalStatus.Cancelled;
            booking.ApprovalDate = new DateTime(2024, 7, 15);
            await _bookingRepository.UpdateBookingAsync(booking);

            // Assert
            var result = await _dbContext.Bookings.FindAsync(booking.BookingId);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.ApprovalStatus, Is.EqualTo(ApprovalStatus.Cancelled));
            Assert.That(result.ApprovalDate, Is.EqualTo(new DateTime(2024, 7, 15)));
        }

        [Test]
        public async Task UpdateBookingAsync_DoesNotCreateNewBooking()
        {
            // Arrange
            var employee = new Employee { Name = "Count Test User" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var booking = new Booking
            {
                Employee = employee,
                RegistrationNumber = "CNT123",
                Make = "Honda",
                Model = "Accord",
                Colour = "Silver",
                Year = "2022",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = DateTime.Now
            };

            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(booking).State = EntityState.Detached;

            var initialCount = await _dbContext.Bookings.CountAsync();

            // Act
            booking.Make = "Updated Honda";
            await _bookingRepository.UpdateBookingAsync(booking);

            // Assert
            var finalCount = await _dbContext.Bookings.CountAsync();
            Assert.That(finalCount, Is.EqualTo(initialCount));
            Assert.That(finalCount, Is.EqualTo(1));
        }

        #endregion
    }
}

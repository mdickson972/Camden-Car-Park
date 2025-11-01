using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camden_Car_Park.WebApi.Data;
using Camden_Car_Park.WebApi.Data.Tables;
using Camden_Car_Park.WebApi.Repositories;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Camden_Car_Park.WebApi.Tests.Repositories
{
    [TestFixture]
    public class EmployeeRepositoryTests
    {
        private CarParkDbContext _dbContext;
        private EmployeeRepository _employeeRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CarParkDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new CarParkDbContext(options);
            _employeeRepository = new EmployeeRepository(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        #region GetAllEmployeesAsync Tests

        [Test]
        public async Task GetAllEmployeesAsync_ReturnsEmptyList_WhenNoEmployeesExist()
        {
            // Act
            var result = await _employeeRepository.GetAllEmployeesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetAllEmployeesAsync_ReturnsSingleEmployee_WhenOneEmployeeExists()
        {
            // Arrange
            var employee = new Employee { Name = "John Doe" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetAllEmployeesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            var employeeList = result.ToList();
            Assert.That(employeeList, Has.Count.EqualTo(1));
            Assert.That(employeeList[0].Name, Is.EqualTo("John Doe"));
            Assert.That(employeeList[0].EmployeeId, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetAllEmployeesAsync_ReturnsAllEmployees_WhenMultipleEmployeesExist()
        {
            // Arrange
            var employees = new List<Employee>
            {
                new Employee { Name = "John Doe" },
                new Employee { Name = "Jane Smith" },
                new Employee { Name = "Bob Johnson" },
                new Employee { Name = "Alice Williams" }
            };

            await _dbContext.Employees.AddRangeAsync(employees);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetAllEmployeesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            var employeeList = result.ToList();
            Assert.That(employeeList, Has.Count.EqualTo(4));

            // Verify all employees are returned by checking names
            var employeeNames = employeeList.Select(e => e.Name).ToList();
            Assert.That(employeeNames, Contains.Item("John Doe"));
            Assert.That(employeeNames, Contains.Item("Jane Smith"));
            Assert.That(employeeNames, Contains.Item("Bob Johnson"));
            Assert.That(employeeNames, Contains.Item("Alice Williams"));
        }

        [Test]
        public async Task GetAllEmployeesAsync_ReturnsEmployeesWithCorrectIds()
        {
            // Arrange
            var employee1 = new Employee { Name = "First Employee" };
            var employee2 = new Employee { Name = "Second Employee" };

            await _dbContext.Employees.AddRangeAsync(employee1, employee2);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetAllEmployeesAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            var employeeList = result.ToList();
            Assert.That(employeeList, Has.Count.EqualTo(2));
            Assert.That(employeeList.All(e => e.EmployeeId > 0), Is.True);
            Assert.That(employeeList.Select(e => e.EmployeeId).Distinct().Count(), Is.EqualTo(2)); // All IDs are unique
        }

        #endregion

        #region GetEmployeeAsync Tests

        [Test]
        public async Task GetEmployeeAsync_ReturnsNull_WhenEmployeeDoesNotExist()
        {
            // Act
            var result = await _employeeRepository.GetEmployeeAsync(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetEmployeeAsync_ReturnsEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employee = new Employee { Name = "Test Employee" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetEmployeeAsync(employee.EmployeeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmployeeId, Is.EqualTo(employee.EmployeeId));
            Assert.That(result.Name, Is.EqualTo("Test Employee"));
        }

        [Test]
        public async Task GetEmployeeAsync_ReturnsCorrectEmployee_WhenMultipleEmployeesExist()
        {
            // Arrange
            var employee1 = new Employee { Name = "John Doe" };
            var employee2 = new Employee { Name = "Jane Smith" };
            var employee3 = new Employee { Name = "Bob Wilson" };

            await _dbContext.Employees.AddRangeAsync(employee1, employee2, employee3);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetEmployeeAsync(employee2.EmployeeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmployeeId, Is.EqualTo(employee2.EmployeeId));
            Assert.That(result.Name, Is.EqualTo("Jane Smith"));
        }

        [Test]
        public async Task GetEmployeeAsync_ReturnsNull_WhenIdIsZero()
        {
            // Arrange
            var employee = new Employee { Name = "Test Employee" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetEmployeeAsync(0);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetEmployeeAsync_ReturnsNull_WhenIdIsNegative()
        {
            // Arrange
            var employee = new Employee { Name = "Test Employee" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _employeeRepository.GetEmployeeAsync(-1);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetEmployeeAsync_ReturnsEmployeeWithAllProperties()
        {
            // Arrange
            var employee = new Employee { Name = "Complete Test Employee" };
            await _dbContext.Employees.AddAsync(employee);
            await _dbContext.SaveChangesAsync();

            var savedEmployeeId = employee.EmployeeId;

            // Act
            var result = await _employeeRepository.GetEmployeeAsync(savedEmployeeId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.EmployeeId, Is.EqualTo(savedEmployeeId));
            Assert.That(result.Name, Is.EqualTo("Complete Test Employee"));
            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.Not.Empty);
        }

        #endregion
    }
}

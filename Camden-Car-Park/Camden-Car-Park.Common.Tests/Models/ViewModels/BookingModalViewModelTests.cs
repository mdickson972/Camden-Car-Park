using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Camden_Car_Park.Common.Models.Enums;
using Camden_Car_Park.Common.Models.ViewModels;

namespace Camden_Car_Park.Common.Tests.Models.ViewModels
{
    [TestFixture]
    public class BookingModalViewModelTests
    {
        private ValidationContext _validationContext;
        private List<ValidationResult> _validationResults;

        [SetUp]
        public void Setup()
        {
            _validationResults = new List<ValidationResult>();
        }

        private BookingModalViewModel CreateValidViewModel()
        {
            return new BookingModalViewModel
            {
                BookingId = 1,
                EmployeeId = 1,
                VehicleRegistrationNumber = "AB12 CDE",
                VehicleMake = "Toyota",
                VehicleModel = "Camry",
                VehicleColour = "Blue",
                VehicleYear = "2022",
                ApprovalStatus = ApprovalStatus.Approved,
                ApprovalDate = DateTime.Today
            };
        }

        private bool ValidateModel(BookingModalViewModel model)
        {
            _validationContext = new ValidationContext(model);
            _validationResults.Clear();
            return Validator.TryValidateObject(model, _validationContext, _validationResults, true);
        }

        #region Valid Model Tests

        [Test]
        public void Validate_ValidModel_PassesValidation()
        {
            // Arrange
            var model = CreateValidViewModel();

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        [Test]
        public void Validate_ValidModelWithMinimalData_PassesValidation()
        {
            // Arrange
            var model = new BookingModalViewModel
            {
                EmployeeId = 1,
                VehicleRegistrationNumber = "ABC 123",
                VehicleMake = "Ford",
                VehicleModel = "Focus",
                VehicleColour = "Red",
                VehicleYear = "2023",
                ApprovalStatus = ApprovalStatus.Pending
            };

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region EmployeeId Validation Tests

        [Test]
        public void Validate_EmployeeIdIsZero_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.EmployeeId = 0;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults, Has.Count.EqualTo(1));
            Assert.That(_validationResults[0].ErrorMessage, Is.EqualTo("Please select an employee."));
            Assert.That(_validationResults[0].MemberNames, Contains.Item(nameof(BookingModalViewModel.EmployeeId)));
        }

        [Test]
        public void Validate_EmployeeIdIsNegative_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.EmployeeId = -1;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults, Has.Count.EqualTo(1));
            Assert.That(_validationResults[0].ErrorMessage, Is.EqualTo("Please select an employee."));
        }

        [Test]
        public void Validate_EmployeeIdIsValid_PassesValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.EmployeeId = 100;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region VehicleRegistrationNumber Validation Tests

        [Test]
        public void Validate_VehicleRegistrationNumberIsEmpty_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleRegistrationNumber = string.Empty;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleRegistrationNumber))), Is.True);
            Assert.That(_validationResults.First(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleRegistrationNumber))).ErrorMessage, 
                Is.EqualTo("Please enter a registration number."));
        }

        [Test]
        public void Validate_VehicleRegistrationNumberIsNull_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleRegistrationNumber = null!;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleRegistrationNumber))), Is.True);
        }

        [Test]
        [TestCase("AB12 CDE", Description = "Current format")]
        [TestCase("ABC 1234", Description = "Prefix format with space")]
        [TestCase("A123 BCD", Description = "Prefix format variation")]
        [TestCase("A1 BCD", Description = "Short prefix format")]
        public void Validate_VehicleRegistrationNumberValidFormats_PassesValidation(string registrationNumber)
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleRegistrationNumber = registrationNumber;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True, $"Registration '{registrationNumber}' should be valid");
            Assert.That(_validationResults, Is.Empty);
        }

        [Test]
        [TestCase("123", Description = "Numbers only")]
        [TestCase("ABC", Description = "Letters only")]
        [TestCase("AB-12-CDE", Description = "With dashes")]
        [TestCase("AB12CDE123", Description = "Too long")]
        [TestCase("", Description = "Empty string")]
        [TestCase(" ", Description = "Whitespace only")]
        public void Validate_VehicleRegistrationNumberInvalidFormats_FailsValidation(string registrationNumber)
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleRegistrationNumber = registrationNumber;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False, $"Registration '{registrationNumber}' should be invalid");
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleRegistrationNumber))), Is.True);
        }

        #endregion

        #region VehicleMake Validation Tests

        [Test]
        public void Validate_VehicleMakeIsEmpty_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleMake = string.Empty;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleMake))), Is.True);
            Assert.That(_validationResults.First(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleMake))).ErrorMessage, 
                Is.EqualTo("Please enter a vehicle make."));
        }

        [Test]
        public void Validate_VehicleMakeIsNull_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleMake = null!;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleMake))), Is.True);
        }

        [Test]
        public void Validate_VehicleMakeIsValid_PassesValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleMake = "Mercedes-Benz";

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region VehicleModel Validation Tests

        [Test]
        public void Validate_VehicleModelIsEmpty_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleModel = string.Empty;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleModel))), Is.True);
            Assert.That(_validationResults.First(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleModel))).ErrorMessage, 
                Is.EqualTo("Please enter a vehicle model."));
        }

        [Test]
        public void Validate_VehicleModelIsNull_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleModel = null!;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleModel))), Is.True);
        }

        [Test]
        public void Validate_VehicleModelIsValid_PassesValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleModel = "Model S";

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region VehicleColour Validation Tests

        [Test]
        public void Validate_VehicleColourIsEmpty_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleColour = string.Empty;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleColour))), Is.True);
            Assert.That(_validationResults.First(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleColour))).ErrorMessage, 
                Is.EqualTo("Please enter a vehicle colour."));
        }

        [Test]
        public void Validate_VehicleColourIsNull_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleColour = null!;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleColour))), Is.True);
        }

        [Test]
        public void Validate_VehicleColourIsValid_PassesValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleColour = "Midnight Blue";

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region VehicleYear Validation Tests

        [Test]
        public void Validate_VehicleYearIsEmpty_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleYear = string.Empty;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleYear))), Is.True);
            Assert.That(_validationResults.First(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleYear))).ErrorMessage, 
                Is.EqualTo("Please enter a vehicle year."));
        }

        [Test]
        public void Validate_VehicleYearIsNull_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleYear = null!;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleYear))), Is.True);
        }

        [Test]
        public void Validate_VehicleYearIsValid_PassesValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.VehicleYear = "2024";

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region ApprovalStatus Validation Tests

        [Test]
        public void Validate_ApprovalStatusIsNull_FailsValidation()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.ApprovalStatus = null;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.ApprovalStatus))), Is.True);
            Assert.That(_validationResults.First(v => v.MemberNames.Contains(nameof(BookingModalViewModel.ApprovalStatus))).ErrorMessage, 
                Is.EqualTo("Please select an approval status."));
        }

        [Test]
        [TestCase(ApprovalStatus.Pending)]
        [TestCase(ApprovalStatus.Approved)]
        [TestCase(ApprovalStatus.Cancelled)]
        public void Validate_ApprovalStatusIsValid_PassesValidation(ApprovalStatus status)
        {
            // Arrange
            var model = CreateValidViewModel();
            model.ApprovalStatus = status;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.True);
            Assert.That(_validationResults, Is.Empty);
        }

        #endregion

        #region Multiple Validation Errors Tests

        [Test]
        public void Validate_MultipleFieldsInvalid_ReturnsAllErrors()
        {
            // Arrange
            var model = new BookingModalViewModel
            {
                EmployeeId = 0,
                VehicleRegistrationNumber = string.Empty,
                VehicleMake = string.Empty,
                VehicleModel = string.Empty,
                VehicleColour = string.Empty,
                VehicleYear = string.Empty,
                ApprovalStatus = null
            };

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Count, Is.GreaterThanOrEqualTo(7));
            
            // Verify each field has an error
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.EmployeeId))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleRegistrationNumber))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleMake))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleModel))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleColour))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleYear))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.ApprovalStatus))), Is.True);
        }

        [Test]
        public void Validate_SomeFieldsInvalid_ReturnsOnlyRelevantErrors()
        {
            // Arrange
            var model = CreateValidViewModel();
            model.EmployeeId = 0;
            model.VehicleRegistrationNumber = "INVALID";
            model.ApprovalStatus = null;

            // Act
            var isValid = ValidateModel(model);

            // Assert
            Assert.That(isValid, Is.False);
            Assert.That(_validationResults.Count, Is.EqualTo(3));
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.EmployeeId))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.VehicleRegistrationNumber))), Is.True);
            Assert.That(_validationResults.Any(v => v.MemberNames.Contains(nameof(BookingModalViewModel.ApprovalStatus))), Is.True);
        }

        #endregion

        #region Property Default Values Tests

        [Test]
        public void NewInstance_HasCorrectDefaultValues()
        {
            // Arrange & Act
            var model = new BookingModalViewModel();

            // Assert
            Assert.That(model.BookingId, Is.EqualTo(0));
            Assert.That(model.EmployeeId, Is.EqualTo(0));
            Assert.That(model.VehicleRegistrationNumber, Is.EqualTo(string.Empty));
            Assert.That(model.VehicleMake, Is.EqualTo(string.Empty));
            Assert.That(model.VehicleModel, Is.EqualTo(string.Empty));
            Assert.That(model.VehicleColour, Is.EqualTo(string.Empty));
            Assert.That(model.VehicleYear, Is.EqualTo(string.Empty));
            Assert.That(model.ApprovalStatus, Is.Null);
            Assert.That(model.ApprovalDate, Is.EqualTo(DateTime.Today));
        }

        [Test]
        public void ApprovalDate_DefaultsToToday()
        {
            // Arrange & Act
            var model = new BookingModalViewModel();

            // Assert
            Assert.That(model.ApprovalDate.Date, Is.EqualTo(DateTime.Today));
        }

        #endregion
    }
}

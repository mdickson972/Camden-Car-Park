using Camden_Car_Park.Common.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Camden_Car_Park.Common.Models.ReadModels
{
    public class BookingModalViewModel
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Please select an employee.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an employee.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please enter a registration number.")]
        public string VehicleRegistrationNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a vehicle make.")]
        public string VehicleMake { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a vehicle model.")]
        public string VehicleModel { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a vehicle colour.")]
        public string VehicleColour { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a vehicle year.")]
        public string VehicleYear { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please select an approval status.")]
        public ApprovalStatus? ApprovalStatus { get; set; }

        public DateTime ApprovalDate { get; set; } = DateTime.Today;
    }
}

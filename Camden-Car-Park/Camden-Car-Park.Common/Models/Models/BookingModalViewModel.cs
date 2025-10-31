using Camden_Car_Park.Common.Models.Enums;

namespace Camden_Car_Park.Common.Models.ReadModels
{
    public class BookingModalViewModel
    {
        public int BookingId { get; set; }

        public int EmployeeId { get; set; }

        public string VehicleRegistrationNumber { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleColour { get; set; }

        public string VehicleYear { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        public DateTime ApprovalDate { get; set; } = DateTime.Today;
    }
}

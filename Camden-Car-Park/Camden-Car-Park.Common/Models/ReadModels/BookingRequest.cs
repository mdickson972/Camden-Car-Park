using Camden_Car_Park.Common.Models.Enums;

namespace Camden_Car_Park.Common.Models.ReadModels
{
    public class BookingRequest
    {
        public int BookingId { get; set; }

        public required int EmployeeId { get; set; }

        public required string VehicleRegistrationNumber { get; set; }

        public required string VehicleMake { get; set; }

        public required string VehicleModel { get; set; }

        public required string VehicleColour { get; set; }

        public required string VehicleYear { get; set; }

        public required ApprovalStatus ApprovalStatus { get; set; }

        public DateTime ApprovalDate { get; set; }
    }
}

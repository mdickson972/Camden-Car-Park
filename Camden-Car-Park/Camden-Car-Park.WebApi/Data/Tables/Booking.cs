using Camden_Car_Park.WebApi.Models.Enums;

namespace Camden_Car_Park.WebApi.Data.Tables
{
    public class Booking
    {
        public required int BookingId { get; set; }

        public required int EmployeeId { get; set; }

        public required int VehicleId { get; set; }

        public required ApprovalStatus ApprovalStatus { get; set; }

        public DateTime ApprovalDate { get; set; }
    }
}

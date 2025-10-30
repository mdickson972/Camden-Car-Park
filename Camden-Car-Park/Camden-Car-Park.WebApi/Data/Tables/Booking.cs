using Camden_Car_Park.WebApi.Models.Enums;

namespace Camden_Car_Park.WebApi.Data.Tables
{
    public class Booking
    {
        public int BookingId { get; set; }

        public required Employee Employee { get; set; }

        public required Vehicle Vehicle { get; set; }

        public required ApprovalStatus ApprovalStatus { get; set; }

        public DateTime ApprovalDate { get; set; }
    }
}

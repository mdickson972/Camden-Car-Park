using Camden_Car_Park.Common.Models.ReadModels;
using Camden_Car_Park.Common.Models.ViewModels;

namespace Camden_Car_Park.Common.Utilities.ExtensionMethods
{
    public static class BookingExtensions
    {
        public static BookingModalViewModel ToBookingModalViewModel(this BookingResponse? response)
        {
            if (response == null)
                return null;

            return new BookingModalViewModel
            {
                BookingId = response.BookingId,
                EmployeeId = response.EmployeeId,
                VehicleRegistrationNumber = response.VehicleRegistrationNumber,
                VehicleMake = response.VehicleMake,
                VehicleModel = response.VehicleModel,
                VehicleColour = response.VehicleColour,
                VehicleYear = response.VehicleYear,
                ApprovalStatus = response.ApprovalStatus,
                ApprovalDate = response.ApprovalDate
            };
        }
    }
}

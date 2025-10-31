using Camden_Car_Park.Common.Models.ReadModels;
using Camden_Car_Park.WebApi.Data.Tables;

namespace Camden_Car_Park.WebApi.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingAsync(int bookingId);
        Task CreateBookingAsync(EmployeeBooking employeeBooking);
    }
}
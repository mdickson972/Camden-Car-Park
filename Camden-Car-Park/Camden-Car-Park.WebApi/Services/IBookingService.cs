using Camden_Car_Park.WebApi.Data.Tables;
using Camden_Car_Park.WebApi.Models.ReadModels;

namespace Camden_Car_Park.WebApi.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking?> GetBookingAsync(int bookingId);
        Task CreateBookingAsync(EmployeeBooking employeeBooking);
    }
}
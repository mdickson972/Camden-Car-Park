using Camden_Car_Park.Common.Models.ReadModels;

namespace Camden_Car_Park.WebApi.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<BookingResponse>> GetAllBookingsAsync();
        Task<BookingResponse?> GetBookingAsync(int bookingId);
        Task CreateBookingAsync(BookingRequest employeeBooking);
        Task UpdateBookingAsync(BookingRequest employeeBooking);
    }
}
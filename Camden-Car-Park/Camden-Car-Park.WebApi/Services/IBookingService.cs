using Camden_Car_Park.Common.Models.ReadModels;
using Camden_Car_Park.WebApi.Data.Tables;

namespace Camden_Car_Park.WebApi.Services
{
    public interface IBookingService
    {
        Task<IEnumerable<EmployeeBooking>> GetAllBookingsAsync();
        Task<EmployeeBooking?> GetBookingAsync(int bookingId);
        Task CreateBookingAsync(EmployeeBooking employeeBooking);
    }
}
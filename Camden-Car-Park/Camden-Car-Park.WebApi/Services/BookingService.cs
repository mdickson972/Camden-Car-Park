using Camden_Car_Park.WebApi.Data.Tables;
using Camden_Car_Park.WebApi.Models.ReadModels;
using Camden_Car_Park.WebApi.Repositories;

namespace Camden_Car_Park.WebApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IEmployeeRepository employeeRepository)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _bookingRepository.GetAllBookingsAsync();
        }

        public async Task<Booking?> GetBookingAsync(int bookingId)
        {
            return await _bookingRepository.GetBookingAsync(bookingId);
        }

        public async Task CreateBookingAsync(EmployeeBooking employeeBooking)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(employeeBooking.EmployeeId);

            var booking = new Booking
            {
                BookingId = employeeBooking.BookingId,
                Employee = employee,
                RegistrationNumber = employeeBooking.VehicleRegistrationNumber,
                Make = employeeBooking.VehicleMake,
                Model = employeeBooking.VehicleModel,
                Colour = employeeBooking.VehicleColour,
                Year = employeeBooking.VehicleYear,
                ApprovalStatus = employeeBooking.ApprovalStatus,
                ApprovalDate = employeeBooking.ApprovalDate
            };

            await _bookingRepository.AddBookingAsync(booking);
        }
    }
}

using Camden_Car_Park.Common.Models.ReadModels;
using Camden_Car_Park.WebApi.Data.Tables;
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

        public async Task<IEnumerable<BookingResponse>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllBookingsAsync();

            return bookings.Select(s => new BookingResponse()
            {
                BookingId = s.BookingId,
                EmployeeId = s.Employee.EmployeeId,
                EmployeeName = s.Employee.Name,
                VehicleRegistrationNumber = s.RegistrationNumber,
                VehicleMake = s.Make,
                VehicleModel = s.Model,
                VehicleColour = s.Colour,
                VehicleYear = s.Year,
                ApprovalStatus = s.ApprovalStatus,
                ApprovalDate = s.ApprovalDate
            });
        }

        public async Task<BookingResponse?> GetBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetBookingAsync(bookingId);

            if (booking == null) { return null; }

            return new BookingResponse()
            {
                BookingId = booking.BookingId,
                EmployeeId = booking.Employee.EmployeeId,
                EmployeeName = booking.Employee.Name,
                VehicleRegistrationNumber = booking.RegistrationNumber,
                VehicleMake = booking.Make,
                VehicleModel = booking.Model,
                VehicleColour = booking.Colour,
                VehicleYear = booking.Year,
                ApprovalStatus = booking.ApprovalStatus,
                ApprovalDate = booking.ApprovalDate
            };
        }

        public async Task CreateBookingAsync(BookingRequest employeeBooking)
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

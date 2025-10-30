using Camden_Car_Park.WebApi.Data.Tables;
using Camden_Car_Park.WebApi.Models.ReadModels;
using Camden_Car_Park.WebApi.Repositories;

namespace Camden_Car_Park.WebApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IEmployeeRepository employeeRepository,
            IVehicleRepository vehicleRepository)
        {
            _bookingRepository = bookingRepository;
            _employeeRepository = employeeRepository;
            _vehicleRepository = vehicleRepository;
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

            var vehicle = await _vehicleRepository.GetVehicleAsync(employeeBooking.VehicleRegistrationNumber);
            // If vehicle does not exist, create a new one
            if (vehicle == null)
            {
                vehicle = new Vehicle
                {
                    RegistrationNumber = employeeBooking.VehicleRegistrationNumber,
                    Make = employeeBooking.VehicleMake,
                    Model = employeeBooking.VehicleModel,
                    Colour = employeeBooking.VehicleColour,
                    Year = employeeBooking.VehicleYear
                };

                await _vehicleRepository.AddVehicleAsync(vehicle);
            }

            var booking = new Booking
            {
                BookingId = employeeBooking.BookingId,
                Employee = employee,
                Vehicle = vehicle,
                ApprovalStatus = employeeBooking.ApprovalStatus,
                ApprovalDate = employeeBooking.ApprovalDate
            };
            await _bookingRepository.AddBookingAsync(booking);
        }
    }
}

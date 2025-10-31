using Camden_Car_Park.Common.Models.ReadModels;
using Camden_Car_Park.WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Camden_Car_Park.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetBookings")]
        public async Task<IResult> Get()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();

            return bookings != null ? Results.Ok(bookings) : Results.NotFound();
        }


        [HttpPost("AddBooking")]
        public async Task<IResult> Post([FromBody] BookingRequest employeeBooking)
        {
            await _bookingService.CreateBookingAsync(employeeBooking);
            return Results.Ok();
        }
    }
}

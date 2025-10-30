using Camden_Car_Park.WebApi.Data;
using Camden_Car_Park.WebApi.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace Camden_Car_Park.WebApi.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly CarParkDbContext _dbContext;

        public BookingRepository(CarParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Booking?> GetBookingAsync(int id)
        {
            return await _dbContext.Bookings.FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings.ToListAsync();
        }

        public async Task AddBookingAsync(Booking booking)
        {
            await _dbContext.Bookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            _dbContext.Bookings.Update(booking);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBookingAsync(Booking booking)
        {
            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();
        }
    }
}

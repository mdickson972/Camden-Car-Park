using Camden_Car_Park.WebApi.Data.Tables;

namespace Camden_Car_Park.WebApi.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking?> GetBookingAsync(int id);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
    }
}
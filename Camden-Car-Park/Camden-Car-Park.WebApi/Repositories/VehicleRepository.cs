using Camden_Car_Park.WebApi.Data;
using Camden_Car_Park.WebApi.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace Camden_Car_Park.WebApi.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly CarParkDbContext _dbContext;

        public VehicleRepository(CarParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Vehicle?> GetVehicleAsync(int id)
        {
            return await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.VehicleId == id);
        }

        public async Task<Vehicle?> GetVehicleAsync(string regNumber)
        {
            return await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.RegistrationNumber == regNumber);
        }

        public async Task<IEnumerable<Vehicle>> GetAllVehiclesAsync()
        {
            return await _dbContext.Vehicles.ToListAsync();
        }

        public async Task AddVehicleAsync(Vehicle vehicle)
        {
            await _dbContext.Vehicles.AddAsync(vehicle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateVehicleAsync(Vehicle vehicle)
        {
            _dbContext.Vehicles.Update(vehicle);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVehicleAsync(Vehicle vehicle)
        {
            _dbContext.Vehicles.Remove(vehicle);
            await _dbContext.SaveChangesAsync();
        }
    }
}

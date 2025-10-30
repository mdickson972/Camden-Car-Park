using Camden_Car_Park.WebApi.Data.Tables;

namespace Camden_Car_Park.WebApi.Repositories
{
    public interface IVehicleRepository
    {
        Task AddVehicleAsync(Vehicle vehicle);
        Task DeleteVehicleAsync(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
        Task<Vehicle?> GetVehicleAsync(int id);
        Task<Vehicle?> GetVehicleAsync(string regNumber);
        Task UpdateVehicleAsync(Vehicle vehicle);
    }
}
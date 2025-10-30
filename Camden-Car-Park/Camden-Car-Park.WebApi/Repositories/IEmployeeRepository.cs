using Camden_Car_Park.WebApi.Data.Tables;

namespace Camden_Car_Park.WebApi.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee?> GetEmployeeAsync(int id);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    }
}
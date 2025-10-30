using Camden_Car_Park.WebApi.Data;
using Camden_Car_Park.WebApi.Data.Tables;
using Microsoft.EntityFrameworkCore;

namespace Camden_Car_Park.WebApi.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CarParkDbContext _dbContext;

        public EmployeeRepository(CarParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee?> GetEmployeeAsync(int id)
        {
            return await _dbContext.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _dbContext.Employees.ToListAsync();
        }
    }
}

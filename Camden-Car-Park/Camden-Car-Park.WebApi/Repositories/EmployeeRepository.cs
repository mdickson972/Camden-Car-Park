using Camden_Car_Park.WebApi.Data;
using Camden_Car_Park.WebApi.Data.Tables;

namespace Camden_Car_Park.WebApi.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CarParkDbContext _dbContext;

        public EmployeeRepository(CarParkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Employee? GetEmployee(int id)
        {
            return _dbContext.Employees.FirstOrDefault(e => e.EmployeeId == id);
        }
    }
}

using Camden_Car_Park.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Camden_Car_Park.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet("GetEmployee")]
        public async Task<IResult> Get(int employeeId)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(employeeId);

            return employee != null ? Results.Ok(employee) : Results.NotFound();
        }

        [HttpGet("GetAllEmployees")]
        public async Task<IResult> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();

            return employees != null ? Results.Ok(employees) : Results.NotFound();
        }
    }
}

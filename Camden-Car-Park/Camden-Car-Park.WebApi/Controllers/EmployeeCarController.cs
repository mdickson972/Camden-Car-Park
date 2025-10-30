using Camden_Car_Park.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Camden_Car_Park.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeCarController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeCarController(IEmployeeRepository employeeRepository)
        {
                _employeeRepository = employeeRepository;
        }

        [HttpGet(Name = "GetEmployeeCar")]
        public string Get()
        {
            return _employeeRepository.GetEmployee(2)?.Name ?? "No Employee Found";
        }
    }
}

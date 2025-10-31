using Camden_Car_Park.Common.Models.Models;
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

        [HttpGet("GetEmployeesList")]
        public async Task<IResult> GetEmployeesList()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var employeesList = employees.Select(e => new EmployeeListItem
            {
                Id = e.EmployeeId,
                Name = e.Name
            });

            return employeesList != null ? Results.Ok(employeesList) : Results.NotFound();
        }
    }
}

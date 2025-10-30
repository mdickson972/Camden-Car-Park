using Microsoft.AspNetCore.Mvc;

namespace Camden_Car_Park.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeCarController : ControllerBase
    {

        [HttpGet(Name = "GetEmployeeCar")]
        public string Get()
        {
            return "Hello from EmployeeCarController";
        }
    }
}

using ApiEmployee3.Models;
using ApiEmployee3.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmployee3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NameSearchController : ControllerBase
    {
        private readonly IEmployeeRepository _empRepository;

        public NameSearchController(IEmployeeRepository empRepository)
        {
            _empRepository = empRepository;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name)
        {
            try
            {
                var result = await _empRepository.Search(name);
                if (result.Any())
                {
                    return Ok(result);
                }
                return NotFound("No employee with that name./ Нема вработен со тоа име");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }
    }
}

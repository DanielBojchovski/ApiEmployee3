using ApiEmployee3.Models;
using ApiEmployee3.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEmployee3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _empRepository;
        public EmployeesController(IEmployeeRepository empRepository)
        {
            _empRepository = empRepository;
        }

        /*[HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name)
        {
            try
            {
                var result = await _empRepository.Search(name);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }*/

        [HttpGet("{skill}")]
        public async Task<ActionResult<IEnumerable<Employee>>> SearchBySkill(string skill)
        {
            try
            {
                var result = await _empRepository.SearchBySkill(skill);

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _empRepository.Get());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _empRepository.Get(id);

                if (result == null)
                {
                    return NotFound("Employee id does not exist./ Вработен со овој идентификационен број не постои.");
                }

                return result;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database./ Грешка при превземањето на податоци од базата");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployees(Employee employee)
        {
            try
            {
                if (employee == null)
                    return BadRequest();

                var emp = await _empRepository.GetEmployeeByEmail(employee.Email);

                if (emp != null)
                {
                    ModelState.AddModelError("Email", "Employee email already in use./ Веќе имаме вработен со оваа email адреса");
                    return BadRequest(ModelState);
                }

                var createdEmployee = await _empRepository.Create(employee);

                return CreatedAtAction(nameof(GetEmployee),
                    new { id = createdEmployee.EmployeeId }, createdEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new employee record./ Грешка при креирање нов вработен.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.EmployeeId)
                    return BadRequest("Employee ID mismatch./ Неусогласеност помеѓу индентификациониот број и вработениот");

                var employeeToUpdate = await _empRepository.Get(id);

                if (employeeToUpdate == null)
                {
                    return NotFound($"Employee with Id = {id} not found./ Вработен со број = {id} не беше пронајден");
                }

                return await _empRepository.Update(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating employee record./ Грешка при ажурирање на вработениот");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await _empRepository.Get(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found./ Вработен со број = {id} не беше пронајден");
                }

                await _empRepository.Delete(id);

                return Ok($"Employee with Id = {id} deleted./ Вработен со број = {id} е избришан");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting employee record. No employee was deleted./ Грешка при бришење на вработен. Ниту еден вработен не беше избришан");
            }
        }
    }
}

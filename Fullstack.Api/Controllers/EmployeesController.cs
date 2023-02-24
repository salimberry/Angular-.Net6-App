using Fullstack.Api.Data;
using Fullstack.Api.Models;
using Fullstack.Api.Services;
using Fullstack.Api.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
     private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService ?? throw new ArgumentNullException(nameof(employeesService));
        }

        [HttpGet]
        public async Task<ActionResult<Employee[]>> GetAllEmployees()
        {
            var employees = await _employeesService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            var employee = await _employeesService.GetEmployeeIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee([FromBody] Employee employee)
        {
            var validator = new EmployeeValidator();
            var validationResult = await validator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var addedEmployee = await _employeesService.AddEmployeeAsync(employee);
                return Ok(addedEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            var validator = new EmployeeValidator();
            var validationResult = await validator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                await _employeesService.UpdateEmployeeAsync(id, employee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
    
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            try
            {
                await _employeesService.DeleteEmployeeAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
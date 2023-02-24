using Fullstack.Api.Data;
using Fullstack.Api.Models;
using Fullstack.Api.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
        private readonly AppDbContext _context; 
        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var data = await _context.employees.ToListAsync();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            var validator = new EmployeeValidator();
            var validationResult = await validator.ValidateAsync(employeeRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // email or phone already exists in the database
            var existingEmployee = await _context.employees.FirstOrDefaultAsync(e => e.Email == employeeRequest.Email || e.Phone == employeeRequest.Phone);
            if (existingEmployee != null)
            {
                return BadRequest("An employee with the same email or phone already exists.");
            }
            employeeRequest.Id = Guid.NewGuid();
            await _context.employees.AddAsync(employeeRequest);
            await _context.SaveChangesAsync();
            return Ok(employeeRequest);
        }


        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
            var employee=await _context.employees.FirstOrDefaultAsync(n => n.Id == id);

            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee updateEmployeeRequest)
        {
            var validator = new EmployeeValidator();
            var validationResult = await validator.ValidateAsync(updateEmployeeRequest);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var employee = await _context.employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            // Check if the email/phone already exists for another employee
            var existingEmployee = await _context.employees.FirstOrDefaultAsync(e => e.Id != id && e.Email == updateEmployeeRequest.Email);
            if (existingEmployee != null)
            {
                return BadRequest(new { message = "An employee with this email already exists" });
            }

            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Salary = updateEmployeeRequest.Salary;

            await _context.SaveChangesAsync();
            return Ok(employee);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _context.employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }
             _context.employees.Remove(employee)   ;

            await _context.SaveChangesAsync();

            return Ok();

        }


    }
}

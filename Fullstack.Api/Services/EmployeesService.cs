using Fullstack.Api.Data;
using Fullstack.Api.Models;
using Fullstack.Api.Validation;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.Api.Services
{
    public class EmployeesService : IEmployeesService
    {
        private readonly AppDbContext _context;

        public EmployeesService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Employee[]> GetAllEmployeesAsync()
        {
            return await _context.employees.ToArrayAsync();
        }

        public async Task<Employee> GetEmployeeIdAsync(Guid id)
        {
            return await _context.employees.FindAsync(id);
        }

        public async Task<Employee> AddEmployeeAsync(Employee employee)
        {
            var validator = new EmployeeValidator();
            var validationResult = await validator.ValidateAsync(employee);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Employee data is invalid.");
            }

            var existingEmployee = await _context.employees.FirstOrDefaultAsync(e => e.Email == employee.Email || e.Phone == employee.Phone);

            if (existingEmployee != null)
            {
                throw new ArgumentException("An employee with the same email or phone already exists.");
            }

            employee.Id = Guid.NewGuid();
            await _context.employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        public async Task UpdateEmployeeAsync(Guid id, Employee updateEmployeeRequest)
        {
            var validator = new EmployeeValidator();
            var validationResult = await validator.ValidateAsync(updateEmployeeRequest);
            if (!validationResult.IsValid)
            {
                throw new ArgumentException("Employee data is invalid.");
            }

            var employee = await _context.employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist.");
            }

            // Check if the email/phone already exists for another employee
            var existingEmployee = await _context.employees.FirstOrDefaultAsync(e => e.Id != id && e.Email == updateEmployeeRequest.Email);
            if (existingEmployee != null)
            {
                throw new ArgumentException($"An employee with email {updateEmployeeRequest.Email} already exists.");
            }

            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Salary = updateEmployeeRequest.Salary;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(Guid id)
        {
            var employee = await _context.employees.FindAsync(id);

            if (employee == null)
            {
                throw new ArgumentException($"Employee with id {id} does not exist.");
            }

            _context.employees.Remove(employee);

            await _context.SaveChangesAsync();
        }
    }
}

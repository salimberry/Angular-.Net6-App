using Fullstack.Api.Models;

namespace Fullstack.Api.Services
{
    public interface IEmployeesService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeIdAsync(Guid id);
        Task<Employee> AddEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Guid id, Employee updateEmployeeRequest);
        Task DeleteEmployeeAsync(Guid id);
    }
}

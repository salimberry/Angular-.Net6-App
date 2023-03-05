using Fullstack.Api.Data;
using Fullstack.Api.Generic;
using Fullstack.Api.Models;
using Fullstack.Api.Services;
using Fullstack.Api.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace Fullstack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : Controller
    {
       private readonly IEmployeesService _employeesService;
        private readonly IMemoryCache _memoryCache;

        public EmployeesController(IEmployeesService employeesService, IMemoryCache memoryCache)
        {
            _employeesService = employeesService ?? throw new ArgumentNullException(nameof(employeesService));
            _memoryCache = memoryCache;

        }

        [HttpGet]
        public async Task<ActionResult> GetAllEmployees()
        {
            Employee employeeobj = new Employee();

            try
            {

                if (!_memoryCache.TryGetValue(employeeobj, out List<Employee> employees))
                {
                     employees = await _employeesService.GetAllEmployeesAsync();
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024,
                    };
                    _memoryCache.Set(employeeobj, employees, cacheEntryOptions);
                }


                BaseResponse baseResponse = new BaseResponse();
                if (employees.Count>0)
                {
                    baseResponse.ResponseCode = (int)ResponseCode.Success;
                    baseResponse.ResponseMessage = "Record Retrieved Successfully";
                    baseResponse.Data= employees;
                    return Ok(baseResponse);
                }
                else
                {
                    baseResponse.ResponseCode = (int)ResponseCode.NotFound;
                    baseResponse.ResponseMessage = "No Record Found";
                    baseResponse.Data = null;
                    return Ok(baseResponse);
                }
               
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetEmployee(Guid id)
        {
            try
            {
                BaseResponse baseResponse = new BaseResponse();

                Employee employeeobj = new Employee();
                if (!_memoryCache.TryGetValue(employeeobj, out List<Employee> employees))
                {
                    employees = await _employeesService.GetAllEmployeesAsync();
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddMinutes(2),
                        SlidingExpiration = TimeSpan.FromMinutes(2),
                        Size = 1024,
                    };
                    _memoryCache.Set(employeeobj, employees, cacheEntryOptions);
                }

                var employee = await _employeesService.GetEmployeeIdAsync(id);
                if (employee!=null)
                {
                    baseResponse.ResponseCode = (int)ResponseCode.Success;
                    baseResponse.ResponseMessage = "Record Retrieved Successfully";
                    baseResponse.Data = employee;
                    return Ok(baseResponse);
                }
                else
                {
                    baseResponse.ResponseCode = (int)ResponseCode.NotFound;
                    baseResponse.ResponseMessage = "No Record Found";
                    baseResponse.Data = null;
                    return Ok(baseResponse);
                }
               

            }
            catch (Exception ex)
            {

                return null;
            }


        }

        [HttpPost]
        public async Task<ActionResult> AddEmployee([FromBody] Employee employee)
        {
            BaseResponse baseResponse = new BaseResponse();

            try
            {
                var validator = new EmployeeValidator();
                var validationResult = await validator.ValidateAsync(employee);

                if (!validationResult.IsValid)
                {
                    baseResponse.ResponseCode =(int)ResponseCode.BadRequest;
                    baseResponse.ResponseMessage = "Bad Request";
                    baseResponse.Data = null;
                    return Ok(baseResponse);
                }else
                {
                    var addedEmployee = await _employeesService.AddEmployeeAsync(employee);
                    if (addedEmployee!=null)
                    {
                        baseResponse.ResponseCode = (int)ResponseCode.Success;
                        baseResponse.ResponseMessage = "Record Retrieved Successfully";
                        baseResponse.Data = employee;
                        return Ok(baseResponse);
                    }
                    else
                    {
                        baseResponse.ResponseCode = (int)ResponseCode.InternalServerError;
                        baseResponse.ResponseMessage = "Failed to save Record";
                        baseResponse.Data = null;
                        return Ok(baseResponse);
                    }
                }

               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody] Employee employee)
        {
            BaseResponse baseResponse = new BaseResponse();


            try
            {
                var validator = new EmployeeValidator();
                var validationResult = await validator.ValidateAsync(employee);

                if (!validationResult.IsValid)
                {
                    baseResponse.ResponseCode = (int)ResponseCode.BadRequest;
                    baseResponse.ResponseMessage = "Bad Request";
                    baseResponse.Data = null;
                }
                else
                {
                    await _employeesService.UpdateEmployeeAsync(id, employee);

                  
                        baseResponse.ResponseCode = (int)ResponseCode.Success;
                        baseResponse.ResponseMessage = "Updated Successfully";
                        baseResponse.Data = employee;
                    return Ok(baseResponse);
                }

                return Ok(baseResponse);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
    
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            BaseResponse baseResponse = new BaseResponse();

            try
            {
                await _employeesService.DeleteEmployeeAsync(id);
                baseResponse.ResponseCode = (int)ResponseCode.Success;
                baseResponse.ResponseMessage = "Deleted Successfully";
                baseResponse.Data = id;
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
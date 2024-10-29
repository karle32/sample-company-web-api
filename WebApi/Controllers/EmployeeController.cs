using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;
using Serilog;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class EmployeeController: ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> GetEmployeeByCode(string employeeCode)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<EmployeeDto>(employee));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while getting employee by code {EmployeeCode}", employeeCode);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                var employeeDtos = employees.Select(e => _mapper.Map<EmployeeDto>(e));
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all employees");
                return InternalServerError(ex);
            }
        }

        public async Task<IHttpActionResult> SaveEmployeeAsync([FromBody] EmployeeDto employee)
        {
            try
            {
                if (employee == null)
                {
                    Log.Warning("Received a null EmployeeDto in SaveEmployeeAsync request.");
                    return BadRequest("Employee data must not be null.");
                }

                var employeeToSave = _mapper.Map<EmployeeInfo>(employee);
                bool result = await _employeeService.SaveEmployeeAsync(employeeToSave);

                if (result)
                {
                    return Ok("Employee saved successfully.");
                }
                else
                {
                    Log.Error("Error occurred while saving employee");
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while saving employee");
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("{employeeCode}")]
        public async Task<IHttpActionResult> DeleteEmployeeAsync(string employeeCode)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (employee == null)
                {
                    return NotFound();
                }

                bool result = await _employeeService.DeleteEmployeeAsync(employee);

                if (result)
                {
                    return Ok("Employee deleted successfully.");
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting employee with code {EmployeeCode}", employeeCode);
                return InternalServerError(ex);
            }
        }
    }
}
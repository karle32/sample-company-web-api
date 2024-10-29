using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
        }

        public async Task<bool> DeleteEmployeeAsync(Employee employee)
        {
            return await _employeeDbWrapper.DeleteAsync(e => e.EmployeeCode == employee.EmployeeCode && e.CompanyCode == employee.CompanyCode);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeDbWrapper.FindAllAsync();
        }

        public async Task<Employee> GetEmployeeByCode(string employeeCode)
        {
            var result = await _employeeDbWrapper.FindAsync(e => e.EmployeeCode.Equals(employeeCode));
            return result?.FirstOrDefault();
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
           var itemRepo = (await _employeeDbWrapper
                .FindAsync(e => e.EmployeeCode == employee.EmployeeCode && e.CompanyCode == employee.CompanyCode))
                .FirstOrDefault();

            if (itemRepo != null) {
                itemRepo.EmployeeName = employee.EmployeeName;
                itemRepo.Occupation = employee.Occupation;
                itemRepo.EmployeeStatus = employee.EmployeeStatus;
                itemRepo.EmailAddress = employee.EmailAddress;
                itemRepo.Phone = employee.Phone;
                itemRepo.LastModified = employee.LastModified;

                return await _employeeDbWrapper.UpdateAsync(itemRepo);
            }

            return await _employeeDbWrapper.InsertAsync(employee);            
        }
    }
}

﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models;

namespace BusinessLayer.Model.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode);
        Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo);
        Task<bool> DeleteEmployeeAsync(EmployeeInfo employeeInfo);
    }
}

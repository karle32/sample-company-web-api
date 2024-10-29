﻿using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByCode(string employeeCode);
        Task<bool> SaveEmployeeAsync(Employee employee);
        Task<bool> DeleteEmployeeAsync(Employee employee);
    }
}

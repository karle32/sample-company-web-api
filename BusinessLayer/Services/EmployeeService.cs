﻿using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<bool> DeleteEmployeeAsync(EmployeeInfo employeeInfo)
        {
            var employee = _mapper.Map<Employee>(employeeInfo);
            return await _employeeRepository.DeleteEmployeeAsync(employee);
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            var res = await _employeeRepository.GetAllEmployeesAsync();
            return _mapper.Map<IEnumerable<EmployeeInfo>>(res);
        }

        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode)
        {
            var result = await _employeeRepository.GetEmployeeByCode(employeeCode);
            return _mapper.Map<EmployeeInfo>(result);
        }

        public async Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo)
        {
            var employee = _mapper.Map<Employee>(employeeInfo);
            return await _employeeRepository.SaveEmployeeAsync(employee);
        }
    }
}

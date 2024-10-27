using System;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }
        // GET api/<controller>
        public IEnumerable<CompanyDto> GetAll()
        {
            var items = _companyService.GetAllCompanies();
            return _mapper.Map<IEnumerable<CompanyDto>>(items);
        }

        // GET api/<controller>/5
        public CompanyDto Get(string companyCode)
        {
            var item = _companyService.GetCompanyByCode(companyCode);
            return _mapper.Map<CompanyDto>(item);
        }

        // POST api/<controller>
        public bool Post([FromBody] CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                {
                    throw new ArgumentNullException(nameof(companyDto), "Company data must not be null.");
                }

                var compnayInfo = _mapper.Map<CompanyInfo>(companyDto);
                return _companyService.SaveCompany(compnayInfo);
            }
            catch (Exception ex)
            {
                // Implement a Logger: Logger.LogError(ex, "An error occurred while deleting the company.");
                throw new InvalidOperationException("An error occurred while saving the company.", ex);
            }
        }

        // PUT api/<controller>/5
        public bool Put(string companyCode, [FromBody]CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                {
                    throw new ArgumentNullException(nameof(companyDto), "Company data must not be null.");
                }

                var existingCompany = _companyService.GetCompanyByCode(companyCode);
                if (existingCompany == null)
                {
                    throw new InvalidOperationException($"Company with code {companyCode} not found.");
                }

                // Map updated values
                var companyInfo = _mapper.Map(companyDto, existingCompany);
                return _companyService.SaveCompany(companyInfo);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                throw new InvalidOperationException("An error occurred while updating the company.", ex);
            }
        }

        // DELETE api/<controller>/5
        public bool Delete(string companyCode)
        {
            try
            {
                var company = _companyService.GetCompanyByCode(companyCode);
                if (company == null)  return false;
                
                return _companyService.DeleteCompany(company);

            }
            catch (Exception ex)
            {
                // Implement a Logger: Logger.LogError(ex, "An error occurred while deleting the company.");
                throw new InvalidOperationException("An error occurred while deleting the company.", ex);
            }
        }
    }
}
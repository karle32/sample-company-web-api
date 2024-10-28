using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<IEnumerable<CompanyDto>> GetAll()
        {
            var items = await _companyService.GetAllCompaniesAsync();
            return _mapper.Map<IEnumerable<CompanyDto>>(items);
        }

        // GET api/<controller>/5
        public async Task<CompanyDto> Get(string companyCode)
        {
            var item = await _companyService.GetCompanyByCodeAsync(companyCode);
            return _mapper.Map<CompanyDto>(item);
        }

        // POST api/<controller>
        [Route("api/company")]
        [HttpPost]
        public async Task<string> Post([FromBody] CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                {
                    return "BadRequest: Company data must not be null.";
                }

                var compnayInfo = _mapper.Map<CompanyInfo>(companyDto);
                bool result = await _companyService.SaveCompanyAsync(compnayInfo);

                return result ? "Company saved successfully." : "Error: An error occurred while saving the company.";
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return "Error: An error occurred while processing your request.";
            }
        }

        // PUT api/<controller>/5
        [Route("api/company/{companyCode}")]
        [HttpPut]
        public async Task<string> Put(string companyCode, [FromBody]CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null) return "BadRequest: Company data must not be null.";

                var existingCompany = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (existingCompany == null)
                {
                    return $"NotFound: Company with code {companyCode} not found.";
                }

                // Map updated values
                var companyInfo = _mapper.Map(companyDto, existingCompany);
                bool result = await _companyService.SaveCompanyAsync(companyInfo);

                return result ? "Company updated successfully." : "Error: An error occurred while updating the company.";
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return "Error: An error occurred while processing your request.";
            }
        }

        // DELETE api/<controller>/5
        [Route("api/company/{companyCode}")]
        [HttpDelete]
        public async Task<string>Delete(string companyCode)
        {
            try
            {
                var company = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (company == null) return $"NotFound: Company with code {companyCode} not found.";

                bool result = await _companyService.DeleteCompanyAsync(company);
                return result ? "Company deleted successfully." : "Error: An error occurred while deleting the company.";
            }
            catch (Exception ex)
            {
                // Log exception (ex)
                return "Error: An error occurred while processing your request.";
            }
        }
    }
}
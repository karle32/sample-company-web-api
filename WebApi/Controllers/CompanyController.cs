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
        [Route("api/company")]
        [HttpPost]
        public IHttpActionResult Post([FromBody] CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                {
                    return BadRequest("Company data must not be null.");
                }

                var compnayInfo = _mapper.Map<CompanyInfo>(companyDto);
                bool isSaved = _companyService.SaveCompany(compnayInfo);

                if (isSaved)
                {
                    return Ok("Company saved successfully");
                }
                else
                {
                    return BadRequest("Failed to save the company");
                }
            }
            catch (Exception ex)
            {
                // Log the exception here
                // Logger.LogError(ex, "An error occurred while saving the company.");
                return InternalServerError(new Exception("An error has occurred on the server. Please try again later. ", ex));
            }
        }

        // PUT api/<controller>/5
        [Route("api/company/{companyCode}")]
        [HttpPut]
        public IHttpActionResult Put(string companyCode, [FromBody]CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null) return BadRequest("Company data must not be Null");

                var existingCompany = _companyService.GetCompanyByCode(companyCode);
                if (existingCompany == null)
                {
                    return NotFound();
                }

                // Map updated values
                var companyInfo = _mapper.Map(companyDto, existingCompany);
                var isSaved = _companyService.SaveCompany(companyInfo);

                if (isSaved)
                {
                    return Ok("Company updated successfully");
                }
                else
                {
                    return BadRequest("Company update failed");
                }
                
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return InternalServerError(new InvalidOperationException("An error occurred while updating the company.", ex));
            }
        }

        // DELETE api/<controller>/5
        [Route("api/company/{companyCode}")]
        [HttpDelete]
        public IHttpActionResult Delete(string companyCode)
        {
            try
            {
                var company = _companyService.GetCompanyByCode(companyCode);
                if (company == null)  return NotFound();

                var isDeleted = _companyService.DeleteCompany(company);
                if (isDeleted)
                {
                    return Ok("Company deleted successfully");
                }
                else
                {
                    return BadRequest("Failed to delete the company");
                }
            }
            catch (Exception ex)
            {
                // Log the exception here.
                // Logger.LogError(ex, "An error occurred while deleting the company.");
                // Return a custom 500 error message
                return InternalServerError(new Exception("An error has occurred on the server. Please try again later.", ex));
            }
        }
    }
}
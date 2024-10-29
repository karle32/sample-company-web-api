using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using Serilog;
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
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var companies = await _companyService.GetAllCompaniesAsync();
                return Ok(_mapper.Map<IEnumerable<CompanyDto>>(companies));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all companies.");
                return InternalServerError(ex);
            }
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("{companyCode}")]
        public async Task<IHttpActionResult> Get(string companyCode)
        {
            try
            {
                var company = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (company == null)
                {
                    Log.Warning("Company with code {CompanyCode} not found.", companyCode);
                    return NotFound();
                }
                return Ok(_mapper.Map<CompanyDto>(company));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving company by code {CompanyCode}", companyCode);
                return InternalServerError(ex);
            }
        }

        // POST api/<controller>
        [Route("api/company")]
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            try
            {

                if (companyDto == null)
                {
                    Log.Warning("Invalid company data submitted.");
                    return BadRequest("Company data must not be null.");
                }

                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                bool result = await _companyService.SaveCompanyAsync(companyInfo);

                if (result)
                {
                    return Ok("Company saved successfully.");
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while saving company.");
                return InternalServerError(ex);
            }
        }

        // PUT api/<controller>/5
        [Route("api/company/{companyCode}")]
        [HttpPut]
        public async Task<IHttpActionResult> Put(string companyCode, [FromBody]CompanyDto companyDto)
        {
            try
            {
                if (companyDto == null)
                {
                    Log.Warning("Invalid company data submitted.");
                    return BadRequest("Company data must not be null.");
                }

                var existingCompany = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (existingCompany == null)
                {
                    Log.Warning("Company with code {CompanyCode} not found.", companyCode);
                    return NotFound();
                }

                var companyInfo = _mapper.Map(companyDto, existingCompany);
                bool result = await _companyService.SaveCompanyAsync(companyInfo);

                if (result) {
                    return Ok("Company updated successfully.");
                }
                else {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating company with code {CompanyCode}", companyCode);
                return InternalServerError(ex);
            }
        }

        // DELETE api/<controller>/5
        [Route("api/company/{companyCode}")]
        [HttpDelete]
        public async Task<IHttpActionResult>Delete(string companyCode)
        {
            try
            {
                var company = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (company == null)
                {
                    Log.Warning("Company with code {CompanyCode} not found.", companyCode);
                    return NotFound();
                }

                bool result = await _companyService.DeleteCompanyAsync(company);
                if (result)
                {
                    return Ok("Company updated successfully.");
                }
                else
                {
                    return StatusCode(HttpStatusCode.InternalServerError);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting company with code {CompanyCode}", companyCode);
                return InternalServerError(ex);
            }
        }
    }
}
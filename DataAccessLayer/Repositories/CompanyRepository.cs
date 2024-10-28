using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
	    private readonly IDbWrapper<Company> _companyDbWrapper;

	    public CompanyRepository(IDbWrapper<Company> companyDbWrapper)
	    {
		    _companyDbWrapper = companyDbWrapper;
        }        

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _companyDbWrapper.FindAllAsync();
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            var result = await _companyDbWrapper.FindAsync(t => t.CompanyCode.Equals(companyCode));
            return result?.FirstOrDefault();
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            var items = await _companyDbWrapper.FindAsync(t =>
                t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));
            var itemRepo = items?.FirstOrDefault();

            if (itemRepo !=null)
            {
                itemRepo.CompanyName = company.CompanyName;
                itemRepo.AddressLine1 = company.AddressLine1;
                itemRepo.AddressLine2 = company.AddressLine2;
                itemRepo.AddressLine3 = company.AddressLine3;
                itemRepo.Country = company.Country;
                itemRepo.EquipmentCompanyCode = company.EquipmentCompanyCode;
                itemRepo.FaxNumber = company.FaxNumber;
                itemRepo.PhoneNumber = company.PhoneNumber;
                itemRepo.PostalZipCode = company.PostalZipCode;
                itemRepo.LastModified = company.LastModified;
                return await _companyDbWrapper.UpdateAsync(itemRepo);
            }

            return await _companyDbWrapper.InsertAsync(company);
        }

        public async Task<bool> DeleteCompanyAsync(Company company)
        {
            var companies = await _companyDbWrapper.FindAsync(t =>
                t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));

            var companyToDelete = companies?.FirstOrDefault();

            if (companyToDelete != null)
            {
                return await _companyDbWrapper.DeleteAsync(t =>
                t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode));
            }
            return await Task.FromResult(false);

        }
    }
}

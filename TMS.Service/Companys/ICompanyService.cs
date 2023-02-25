using System.Collections.Generic;
using TMS.Core.Domains;

namespace TMS.Service.Companys
{
    public partial interface ICompanyService
    {
        IList<Company> GetAll(int tenantId);

        Company GetById(int Id, int tenantId);
    }
}
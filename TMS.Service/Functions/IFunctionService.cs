using System.Collections.Generic;
using TMS.Core.Domains;

namespace TMS.Service
{
    /// <summary>
    /// Function service interface
    /// </summary>
    public partial interface IFunctionService
    {
        IList<Function> GetAllFuntions(int? parentID = null, int companyID = 0, int tenantID = 0);
    }
}
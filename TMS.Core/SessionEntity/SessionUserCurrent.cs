using System;
using System.Collections.Generic;
using TMS.Core.Extend;

namespace TMS.Core.SessionEntity
{
    [Serializable]
    public class SessionUserCurrent
    {
        public SessionUserCurrent()
        {
            Companys = new List<UserCompany>();
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string AvartarUrl { get; set; }

        public string JobTitle { get; set; }

        public bool IsTenant { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public List<UserCompany> Companys { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
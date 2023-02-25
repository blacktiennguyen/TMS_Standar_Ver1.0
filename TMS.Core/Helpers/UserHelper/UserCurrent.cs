using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains;
using TMS.Core.Extend;

namespace TMS.Core
{
    public static class UserCurrent
    {
        public static int UserId { get; set; }

        public static string UserName { get; set; }

        public static string FullName { get; set; }

        public static string JobTitle { get; set; }

        public static string AvartarUrl { get; set; }

        public static int CompanyId { get; set; }

        public static int TenantId { get; set; }

        public static bool IsTenant { get; set; }

        public static List<UserCompany> Companys { get; set; }

        public static DateTime CreatedDate { get; set; }
    }
}
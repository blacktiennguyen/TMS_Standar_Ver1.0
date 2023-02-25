using System;
using System.Collections.Generic;

namespace TMS.Core.Domains
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string AvartarUrl { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string JobTitle { get; set; }

        public bool IsUseKeyLogin { get; set; }

        public Guid? KeyLogin { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsTenant { get; set; }

        public string Remark { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public int? CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
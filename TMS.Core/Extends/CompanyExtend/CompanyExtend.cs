using System;

namespace TMS.Core.Extend
{
    public class CompanyExtend
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public int UserNumber { get; set; }

        public string Remark { get; set; }

        public int TenantId { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
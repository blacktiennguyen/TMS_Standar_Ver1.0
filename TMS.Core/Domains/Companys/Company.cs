using System;

namespace TMS.Core.Domains
{
    public class Company : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public bool IsActive { get; set; }

        public int UserNumber { get; set; }

        public string LogoUrl { get; set; }

        public int? CountryId { get; set; }

        public int? LocalCurrencyId { get; set; }

        public string Remark { get; set; }

        public int TenantId { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
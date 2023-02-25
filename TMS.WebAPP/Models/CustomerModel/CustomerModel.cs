using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.WebAPP.Framework.Mvc;

namespace TMS.WebAPP.Models
{
    public class CustomerModel : BaseTMSEntityModel
    {
        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string IdentityCardNumber { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? GenderId { get; set; }

        public bool? IsCompany { get; set; }

        public string Email { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Fax { get; set; }

        public string TaxCode { get; set; }

        public string Website { get; set; }

        public int? CountryId { get; set; }

        public int? ProvinceId { get; set; }

        public int? DistrictId { get; set; }

        public int? WardId { get; set; }

        public string FullAddress { get; set; }
        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
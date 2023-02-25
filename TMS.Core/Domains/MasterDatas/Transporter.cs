using System;
using System.Collections.Generic;

namespace TMS.Core.Domains.MasterDatas
{
    public class Transporter : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid? TranslationId { get; set; }

        public string Remark { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public int? CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
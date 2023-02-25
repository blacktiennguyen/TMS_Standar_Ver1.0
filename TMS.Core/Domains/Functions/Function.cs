using System;

namespace TMS.Core.Domains
{
    public class Function : BaseEntity
    {
        public string Name { get; set; }

        public bool? IsActive { get; set; }

        public int? ParentId { get; set; }

        public string Remark { get; set; }

        public string CssIcon { get; set; }

        public int DisplayOrder { get; set; }

        public int LevelFunction { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public Guid TranslationId { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
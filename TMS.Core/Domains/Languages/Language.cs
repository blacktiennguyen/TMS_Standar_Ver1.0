using System;

namespace TMS.Core.Domains
{
    public class Language : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public string Icon { get; set; }

        public int DisplayOrder { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
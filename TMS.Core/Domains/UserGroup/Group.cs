using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core.Domains
{
    public class Group : BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Remark { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
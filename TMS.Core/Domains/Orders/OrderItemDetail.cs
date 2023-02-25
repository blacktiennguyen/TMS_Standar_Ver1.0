using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core.Domains
{
    public class OrderItemDetail : BaseEntity
    {
        public int OrderId { get; set; }

        public string ItemName { get; set; }

        public double? Amount { get; set; }

        public int? ItemUnitId { get; set; }

        public double? Weight { get; set; }

        public int? WeightTypeId { get; set; }

        public int? ItemTypeId { get; set; }

        public double? ItemLength { get; set; }

        public int? ItemLengthLengthTypeId { get; set; }

        public double? ItemWidth { get; set; }

        public int? ItemWidthLengthTypeId { get; set; }

        public double? ItemHeight { get; set; }

        public int? ItemHeightLengthTypeId { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.WebAPP.Models.Order
{
    public class OrderItemDetailModel
    {
        public int STT { get; set; }
        public int OrderId { get; set; }

        public string ItemName { get; set; }

        public double? Amount { get; set; }

        public int? ItemUnitId { get; set; }

        public string ItemUnit { get; set; }

        public double? Weight { get; set; }

        public int? WeightTypeId { get; set; }

        public string WeightType { get; set; }

        public int? ItemTypeId { get; set; }

        public string ItemType { get; set; }

        public string ItemLength { get; set; }

        public string ItemWidth { get; set; }

        public string ItemHeight { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
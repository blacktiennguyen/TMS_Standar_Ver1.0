using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core.Domains
{
    public class Order : BaseEntity
    {
        public string OrderCode { get; set; }

        public string BillOfLadingCode { get; set; }

        public int CustomerFromId { get; set; }

        public int CustomerToId { get; set; }

        public DateTime OrderCreateDate { get; set; }

        public DateTime ExpectedDeliveryDate { get; set; }

        public DateTime EstimatedDeliveryStartDate { get; set; }

        public DateTime EstimatedDeliveryEndDate { get; set; }

        public int? EstimatedNumberDaysDelivery { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderShippingStatusId { get; set; }

        public int VehicleTypeId { get; set; }

        #region Address From

        public int CountryFromId { get; set; }
        public int ProvinceFromId { get; set; }

        public int DistrictFromId { get; set; }
        public int WardFromId { get; set; }

        public string AddressFrom { get; set; }

        #endregion Address From

        #region Address To

        public int CountryToId { get; set; }
        public int ProvinceToId { get; set; }

        public int DistrictToId { get; set; }

        public int WardToId { get; set; }

        public string AddressTo { get; set; }

        #endregion Address To

        public bool? IsBillOfExchange { get; set; }
        public double? TotalWeight { get; set; }

        public int? WeightTypeId { get; set; }

        public bool? IsCollectingMoney { get; set; }

        public double? TotalCollectingMoney { get; set; }

        public double? TotalOrderValue { get; set; }

        public double? TotalService { get; set; }

        public double? TotalPostage { get; set; }

        public double? TotalTax { get; set; }

        public double? TotalReceivable { get; set; }

        public int? CurrencyId { get; set; }

        public int WorkflowId { get; set; }

        public int TransporterId { get; set; }

        public int PayerPostageServiceId { get; set; }

        public int RouteId { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public string Remark { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedDate { get; set; }

        public int UpdatedById { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.WebAPP.Framework.Mvc;

namespace TMS.WebAPP.Models
{
    public class OrderModel : BaseTMSEntityModel
    {
        [Required]
        public string OrderCode { get; set; }

        public string BillOfLadingCode { get; set; }

        public DateTime OrderCreateDate { get; set; }

        public string OrderCreateDateFormat { get; set; }

        [Required]
        public DateTime ExpectedDeliveryDate { get; set; }

        public string ExpectedDeliveryDateFormat { get; set; }

        [Required]
        public DateTime EstimatedDeliveryStartDate { get; set; }

        public string EstimatedDeliveryStartDateFormat { get; set; }

        public DateTime EstimatedDeliveryEndDate { get; set; }

        public string EstimatedDeliveryEndDateFormat { get; set; }

        public int? EstimatedNumberDaysDelivery { get; set; }

        public int? EstimatedLeadTimeDay { get; set; }
        public int OrderStatusId { get; set; }

        public string OrderStatusName { get; set; }
        public int OrderShippingStatusId { get; set; }

        public string OrderShippingStatusName { get; set; }

        #region Address From

        public int CountryFromId { get; set; }
        public int? ProvinceFromId { get; set; }

        public int? DistrictFromId { get; set; }
        public int? WardFromId { get; set; }
        public string AddressFrom { get; set; }

        #endregion Address From

        #region Address To

        public int CountryToId { get; set; }
        public int? ProvinceToId { get; set; }

        public int? DistrictToId { get; set; }
        public int? WardToId { get; set; }
        public string AddressTo { get; set; }

        public string AddressToGroup { get; set; }

        #endregion Address To

        #region Customer Info

        public string CustomerFrom { get; set; } // show on grid

        public string CustomerTo { get; set; } // show on grid

        #region Customer From

        public int CustomerFromId { get; set; }

        public string CustomerFromCode { get; set; }

        public string CustomerFromName { get; set; }

        public string CustomerFromIdentityCardNumber { get; set; }

        public string CustomerFromPhone1 { get; set; }

        public string CustomerFromPhone2 { get; set; }

        public string CustomerFromTaxCode { get; set; }

        public string CustomerFromFullAddress { get; set; }

        #endregion Customer From

        #region Customer To

        public int CustomerToId { get; set; }

        public string CustomerToCode { get; set; }

        public string CustomerToName { get; set; }

        public string CustomerToIdentityCardNumber { get; set; }

        public string CustomerToPhone1 { get; set; }

        public string CustomerToPhone2 { get; set; }

        public string CustomerToTaxCode { get; set; }

        public string CustomerToFullAddress { get; set; }

        #endregion Customer To

        #endregion Customer Info

        public double? TotalWeight { get; set; }

        public int? WeightTypeId { get; set; }

        public bool IsCollectingMoney { get; set; }

        public double? TotalCollectingMoney { get; set; }

        public int? LocalCurrencyId { get; set; }

        public string LocalCurrency { get; set; }

        public int? CurrencyId { get; set; }

        public double? TotalOrderValue { get; set; }

        public double? TotalService { get; set; }

        public double? TotalPostage { get; set; }

        public double? TotalTax { get; set; }

        public double? TotalReceivable { get; set; }

        public bool IsBillOfExchange { get; set; }

        public int VehicleTypeId { get; set; }

        public int WorkflowId { get; set; }

        public int TransporterId { get; set; }

        public string TransporterName { get; set; }

        public string VehicleTypeName { get; set; }

        public int PayerPostageServiceId { get; set; }

        public int RouteId { get; set; }

        public string RouteName { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }

        public string Remark { get; set; }

        public string CreatedBy { get; set; }

        public string CreatedDate { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        public string SystemFormatDateTime { get; set; }

        #region Search

        public DateTime? SearchExpectedDeliveryDateFrom { get; set; }

        public DateTime? SearchExpectedDeliveryDateTo { get; set; }

        public DateTime? SearchEstimatedDeliveryStartDateFrom { get; set; }

        public DateTime? SearchEstimatedDeliveryStartDateTo { get; set; }

        public DateTime? SearchEstimatedDeliveryEndDateFrom { get; set; }

        public DateTime? SearchEstimatedDeliveryEndDateTo { get; set; }

        #endregion Search
    }
}
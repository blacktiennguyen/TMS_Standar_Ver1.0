using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TMS.Core.Extend
{
    public class OrderSearchExtend
    {
        public string OrderCode { get; set; }

        public string BillOfLadingCode { get; set; }

        public DateTime? SearchExpectedDeliveryDateFrom { get; set; }

        public DateTime? SearchExpectedDeliveryDateTo { get; set; }

        public DateTime? SearchEstimatedDeliveryStartDateFrom { get; set; }

        public DateTime? SearchEstimatedDeliveryStartDateTo { get; set; }

        public DateTime? SearchEstimatedDeliveryEndDateFrom { get; set; }

        public DateTime? SearchEstimatedDeliveryEndDateTo { get; set; }
        public int? OrderStatusId { get; set; }

        public int? OrderShippingStatusId { get; set; }

        #region Address From

        public int? CountryFromId { get; set; }
        public int? ProvinceFromId { get; set; }

        public int? DistrictFromId { get; set; }
        public int? WardFromId { get; set; }

        #endregion Address From

        #region Address To

        public int? CountryToId { get; set; }
        public int? ProvinceToId { get; set; }

        public int? DistrictToId { get; set; }
        public int? WardToId { get; set; }

        #endregion Address To

        #region Customer Info

        #region Customer From

        public string CustomerFromCode { get; set; }

        public string CustomerFromName { get; set; }

        public string CustomerFromIdentityCardNumber { get; set; }

        public string CustomerFromPhone1 { get; set; }

        public string CustomerFromTaxCode { get; set; }

        #endregion Customer From

        #region Customer To

        public string CustomerToCode { get; set; }

        public string CustomerToName { get; set; }

        public string CustomerToIdentityCardNumber { get; set; }

        public string CustomerToPhone1 { get; set; }

        public string CustomerToTaxCode { get; set; }

        #endregion Customer To

        #endregion Customer Info

        public int? VehicleTypeId { get; set; }

        public int? TransporterId { get; set; }
    }
}
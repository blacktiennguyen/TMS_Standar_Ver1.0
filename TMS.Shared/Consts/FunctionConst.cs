using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Shared.Const
{
    public class FunctionConst
    {
        #region Function Id

        public const int Order = 9;
        public const int OrderDataSetup = 69;
        public const int ShippingPlan = 14;
        public const int VehicleDataSetup = 76;

        #endregion Function Id

        #region Function Code Generate

        public const string OrderGenerateCode = "ORD";

        #endregion Function Code Generate

        #region Url Function

        public const string OrderUrl = "/Order/List";

        #region Order Data Setup

        public const string OrderDataSetupUrl = "/OrderDataSetup/Index";

        public const string ItemTypeOrderDataSetupUrl = "/ItemType/List";

        public const string ItemUnitOrderDataSetupUrl = "/ItemUnit/List";

        public const string TransporterOrderDataSetupUrl = "/Transporter/List";

        #endregion Order Data Setup

        #region Order Data Setup

        public const string VehicleDataSetupUrl = "/VehicleDataSetup/Index";

        public const string VehicleTypeDataSetupUrl = "/VehicleType/List";

        #endregion Order Data Setup

        #endregion Url Function
    }
}
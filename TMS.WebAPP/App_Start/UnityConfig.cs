using System.Web.Mvc;
using TMS.Core;
using TMS.Service;
using TMS.Service.CaptionTranslations;
using TMS.Service.Companys;
using TMS.Service.Customers;
using TMS.Service.FixDataTranslations;
using TMS.Service.FIXs;
using TMS.Service.FunctionTranslations;
using TMS.Service.Languages;
using TMS.Service.MasterDatas;
using TMS.Service.MasterDataTranslations;
using TMS.Service.Orders;
using TMS.Service.SystemSettings;
using TMS.Service.UserGroups;
using TMS.Service.Users;
using TMS.Service.Workflows;
using Unity;
using Unity.Mvc5;

namespace TMS.WebAPP
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            #region Systems

            container.RegisterType<IFunctionService, FunctionService>();

            container.RegisterType<ILanguageService, LanguageService>();

            container.RegisterType<ICompanyService, CompanyService>();

            container.RegisterType<IUserService, UserService>();

            #endregion Systems

            #region Translations

            container.RegisterType<IFunctionTranslationService, FunctionTranslationService>();

            container.RegisterType<ICaptionTranslationService, CaptionTranslationService>();

            container.RegisterType<IFixDataTranslationService, FixDataTranslationService>();

            container.RegisterType<IMasterDataTranslationService, MasterDataTranslationService>();

            #endregion Translations

            #region User Group & Group

            container.RegisterType<IGroupService, GroupService>();

            #endregion User Group & Group

            #region Order

            container.RegisterType<IOrderService, OrderService>();

            container.RegisterType<IOrderItemDetailService, OrderItemDetailService>();

            #endregion Order

            #region FIXs Table

            container.RegisterType<IOrderStatusService, OrderStatusService>();

            container.RegisterType<IOrderShippingStatusService, OrderShippingStatusService>();

            container.RegisterType<IWeightTypeService, WeightTypeService>();

            container.RegisterType<ILengthTypeService, LengthTypeService>();

            container.RegisterType<IPayerPostageServiceService, PayerPostageServiceService>();

            container.RegisterType<ITransportationMethodService, TransportationMethodService>();

            #endregion FIXs Table

            #region Master Datas

            container.RegisterType<IItemTypeService, ItemTypeService>();

            container.RegisterType<IItemUnitService, ItemUnitService>();

            container.RegisterType<ICountryService, CountryService>();

            container.RegisterType<IProvinceService, ProvinceService>();

            container.RegisterType<IDistrictService, DistrictService>();

            container.RegisterType<IWardService, WardService>();

            container.RegisterType<IVehicleTypeService, VehicleTypeService>();

            container.RegisterType<ITransporterService, TransporterService>();

            container.RegisterType<ICurrencyService, CurrencyService>();

            container.RegisterType<IRouteService, RouteService>();

            #endregion Master Datas

            #region Customer

            container.RegisterType<ICustomerService, CustomerService>();

            #endregion Customer

            #region SystemSetting

            container.RegisterType<ISystemSettingService, SystemSettingService>();

            #endregion SystemSetting

            #region WorkflowSetting

            container.RegisterType<IWorkflowSettingService, WorkflowSettingService>();

            #endregion WorkflowSetting

            container.RegisterType<IDbContext, TMSContext>();
            container.RegisterType(typeof(IRepository<>), typeof(EfRepository<>));
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
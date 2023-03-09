using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Extend;
using TMS.Library.Utils;
using TMS.Service.Customers;
using TMS.Service.FixDataTranslations;
using TMS.Service.FIXs;
using TMS.Service.MasterDatas;
using TMS.Service.MasterDataTranslations;
using TMS.Service.Orders;
using TMS.Service.SystemSettings;
using TMS.Service.Users;
using TMS.Shared.Const;
using TMS.Shared.Consts;
using TMS.WebAPP.Framework.Controllers;
using TMS.WebAPP.Models;
using TMS.WebAPP.Models.Order;

namespace TMS.WebAPP.Controllers
{
    public class OrderController : TMSBaseController
    {
        #region Fields

        private readonly IOrderService _orderService;

        private readonly IOrderItemDetailService _orderItemDetailService;

        private readonly IUserService _userService;

        private readonly IOrderStatusService _orderStatusService;

        private readonly IOrderShippingStatusService _orderShippingStatusService;

        private readonly IFixDataTranslationService _fixDataTranslationService;

        private readonly IMasterDataTranslationService _masterDataTranslationService;

        private readonly IWeightTypeService _weightTypeService;

        private readonly ILengthTypeService _lengthTypeService;

        private readonly IItemTypeService _itemTypeService;

        private readonly IItemUnitService _itemUnitService;

        private readonly ICountryService _countryService;

        private readonly IProvinceService _provinceService;

        private readonly IDistrictService _districtService;

        private readonly IWardService _wardService;

        private readonly ICustomerService _customerService;

        private readonly ISystemSettingService _systemSettingService;

        private readonly ICurrencyService _currencyService;

        private readonly IPayerPostageServiceService _payerPostageServiceService;

        private readonly ITransporterService _transporterService;

        private readonly IVehicleTypeService _vehicleTypeService;

        private readonly IRouteService _routeService;

        #endregion Fields

        #region Constructors

        public OrderController(IOrderService orderpService, IUserService userService,
            IOrderStatusService orderStatusService, IFixDataTranslationService fixDataTranslationService,
            IOrderShippingStatusService orderShippingStatusService,
            IWeightTypeService weightTypeService,
            IOrderItemDetailService orderItemDetailService,
            IItemTypeService itemTypeService,
            IMasterDataTranslationService masterDataTranslationService,
            IItemUnitService itemUnitService,
            ICountryService countryService,
            IProvinceService provinceService,
            IDistrictService districtService,
            IWardService wardService,
            ICustomerService customerService,
            ILengthTypeService lenghtTypeService,
            ISystemSettingService systemSettingService,
            ICurrencyService currencyService,
            IPayerPostageServiceService payerPostageServiceService,
            ITransporterService transporterService,
            IVehicleTypeService vehicleTypeService,
            IRouteService routeService)
        {
            this._orderService = orderpService;
            this._userService = userService;
            this._orderStatusService = orderStatusService;
            this._fixDataTranslationService = fixDataTranslationService;
            this._orderShippingStatusService = orderShippingStatusService;
            this._weightTypeService = weightTypeService;
            this._orderItemDetailService = orderItemDetailService;
            this._itemTypeService = itemTypeService;
            this._masterDataTranslationService = masterDataTranslationService;
            this._itemUnitService = itemUnitService;
            this._countryService = countryService;
            this._provinceService = provinceService;
            this._districtService = districtService;
            this._wardService = wardService;
            this._customerService = customerService;
            this._lengthTypeService = lenghtTypeService;
            this._systemSettingService = systemSettingService;
            this._currencyService = currencyService;
            this._payerPostageServiceService = payerPostageServiceService;
            this._transporterService = transporterService;
            this._vehicleTypeService = vehicleTypeService;
            this._routeService = routeService;
        }

        #endregion Constructors

        #region Propertys

        public string dateSystemFormat = "dd-MM-yyyy";

        #endregion Propertys

        #region List

        // GET: GroupUser
        public ActionResult List()
        {
            SetActiveFunction(FunctionConst.OrderUrl);
            SetActiveFunctionDataSetup("");

            OrderModel model = new OrderModel();
            var systemSetting = _systemSettingService.GetByKey(SystemSettingConst.KeyDateFormat, CompanyCurrent.Id, CompanyCurrent.TenantId);

            if (systemSetting != null && systemSetting.Value != null)
                dateSystemFormat = systemSetting.Value;

            model.SearchEstimatedDeliveryEndDateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.SearchEstimatedDeliveryEndDateTo = new DateTime(DateTime.Now.Year, 12, 31);
            model.SearchEstimatedDeliveryStartDateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.SearchEstimatedDeliveryStartDateTo = new DateTime(DateTime.Now.Year, 12, 31);
            model.SearchExpectedDeliveryDateFrom = new DateTime(DateTime.Now.Year, 1, 1);
            model.SearchExpectedDeliveryDateTo = new DateTime(DateTime.Now.Year, 12, 31);
            model.SystemFormatDateTime = dateSystemFormat;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult List(DataSourceRequest command, OrderModel model)
        {
            try
            {
                var orderSearchExtend = new OrderSearchExtend();
                orderSearchExtend = PrepareOrderSearchExtend(model);

                var orders = _orderService.SearchOrder(orderSearchExtend, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);
                var systemSetting = _systemSettingService.GetByKey(SystemSettingConst.KeyDateFormat, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (systemSetting != null && systemSetting.Value != null)
                    dateSystemFormat = systemSetting.Value;

                var gridModel = new DataSourceResult
                {
                    Data = orders.Select(x =>
                    {
                        var createdByUser = _userService.GetById(x.CreatedById);

                        #region Order Status

                        var orderStatusName = GetOrderStatusName(x.OrderStatusId);

                        #endregion Order Status

                        #region Order Shipping Status

                        var orderShippingStatusName = GetOrderShippingStatusName(x.OrderShippingStatusId);

                        #endregion Order Shipping Status

                        #region Transporter

                        var transporterName = GetTransporterName(x.TransporterId);

                        #endregion Transporter

                        #region VehicleType

                        var vehicleTypeName = GetVehicleTypeName(x.TransporterId);

                        #endregion VehicleType

                        #region Route

                        var routeName = GetRouteName(x.RouteId);

                        #endregion Route

                        #region Address From - To

                        var addressFrom = "";
                        var addressTo = "";
                        var addressToGroup = "";

                        var countryFromName = GetCountryName((int)x.CountryFromId);
                        var countryToName = GetCountryName((int)x.CountryToId);
                        var provinceFromName = GetProvinceName((int)x.ProvinceFromId);
                        var provinceToName = GetProvinceName((int)x.ProvinceToId);
                        var districtFromName = GetDistrictName((int)x.DistrictFromId);
                        var districtToName = GetDistrictName((int)x.DistrictToId);
                        var wardFromName = GetWardName((int)x.WardFromId);
                        var wardToName = GetDistrictName((int)x.WardToId);

                        addressFrom = string.Format("{0}", x.AddressFrom);
                        addressTo = string.Format("{0}", x.AddressTo);

                        addressToGroup = string.Format("{0}, {1}", provinceToName, countryToName);

                        #endregion Address From - To

                        return new OrderModel
                        {
                            Id = x.Id,
                            OrderCode = x.OrderCode,
                            BillOfLadingCode = x.BillOfLadingCode,
                            ExpectedDeliveryDateFormat = Convert.ToDateTime(x.ExpectedDeliveryDate).ToString(dateSystemFormat),
                            EstimatedDeliveryStartDateFormat = Convert.ToDateTime(x.EstimatedDeliveryStartDate).ToString(dateSystemFormat),
                            EstimatedDeliveryEndDateFormat = Convert.ToDateTime(x.EstimatedDeliveryEndDate).ToString(dateSystemFormat),
                            OrderCreateDateFormat = Convert.ToDateTime(x.OrderCreateDate).ToString(dateSystemFormat),
                            EstimatedNumberDaysDelivery = x.EstimatedNumberDaysDelivery,
                            OrderStatusName = orderStatusName,
                            OrderStatusId = x.OrderStatusId,
                            OrderShippingStatusId = x.OrderShippingStatusId,
                            OrderShippingStatusName = orderShippingStatusName,
                            AddressFrom = addressFrom,
                            AddressTo = addressTo,
                            AddressToGroup = addressToGroup,
                            CustomerFrom = GetCustomer(x.CustomerFromId),
                            CustomerTo = GetCustomer(x.CustomerToId),
                            TotalOrderValue = x.TotalOrderValue.GetValueOrDefault(),
                            Remark = x.Remark,
                            CreatedBy = createdByUser != null ? createdByUser.UserName : "",
                            TransporterName = transporterName,
                            VehicleTypeName = vehicleTypeName,
                            RouteName = routeName
                        };
                    }),
                    Total = orders.TotalCount
                };

                return Json(gridModel);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return null;
            }
        }

        public OrderSearchExtend PrepareOrderSearchExtend(OrderModel model)
        {
            var orderSearchExtend = new OrderSearchExtend();
            orderSearchExtend.OrderCode = model.OrderCode;
            orderSearchExtend.BillOfLadingCode = model.BillOfLadingCode;
            orderSearchExtend.SearchEstimatedDeliveryEndDateFrom = model.SearchEstimatedDeliveryEndDateFrom;
            orderSearchExtend.SearchEstimatedDeliveryEndDateTo = model.SearchEstimatedDeliveryEndDateTo;
            orderSearchExtend.SearchEstimatedDeliveryStartDateFrom = model.SearchEstimatedDeliveryStartDateFrom;
            orderSearchExtend.SearchEstimatedDeliveryStartDateTo = model.SearchEstimatedDeliveryStartDateTo;
            orderSearchExtend.SearchExpectedDeliveryDateFrom = model.SearchExpectedDeliveryDateFrom;
            orderSearchExtend.SearchExpectedDeliveryDateTo = model.SearchExpectedDeliveryDateTo;

            #region Customer From - To

            orderSearchExtend.CustomerFromCode = model.CustomerFromCode;
            orderSearchExtend.CustomerFromName = model.CustomerFromName;
            orderSearchExtend.CustomerFromPhone1 = model.CustomerFromPhone1;
            orderSearchExtend.CustomerFromIdentityCardNumber = model.CustomerFromIdentityCardNumber;
            orderSearchExtend.CustomerFromTaxCode = model.CustomerFromTaxCode;

            orderSearchExtend.CustomerToCode = model.CustomerToCode;
            orderSearchExtend.CustomerToName = model.CustomerToName;
            orderSearchExtend.CustomerToPhone1 = model.CustomerToPhone1;
            orderSearchExtend.CustomerToIdentityCardNumber = model.CustomerToIdentityCardNumber;
            orderSearchExtend.CustomerToTaxCode = model.CustomerToTaxCode;

            #endregion Customer From - To

            #region Address From - To

            orderSearchExtend.CountryFromId = model.CountryFromId;
            orderSearchExtend.ProvinceFromId = model.ProvinceFromId;
            orderSearchExtend.DistrictFromId = model.DistrictFromId;
            orderSearchExtend.WardFromId = model.WardFromId;

            orderSearchExtend.CountryToId = model.CountryToId;
            orderSearchExtend.ProvinceToId = model.ProvinceToId;
            orderSearchExtend.DistrictFromId = model.DistrictFromId;
            orderSearchExtend.WardFromId = model.WardFromId;

            #endregion Address From - To

            orderSearchExtend.TransporterId = model.TransporterId;
            orderSearchExtend.OrderStatusId = model.OrderStatusId;
            orderSearchExtend.OrderShippingStatusId = model.OrderShippingStatusId;
            orderSearchExtend.VehicleTypeId = model.VehicleTypeId;

            return orderSearchExtend;
        }

        #endregion List

        #region Create / Edit / Delete

        public virtual ActionResult Create()
        {
            SetActiveFunction(FunctionConst.OrderUrl);

            var systemSetting = _systemSettingService.GetByKey(SystemSettingConst.KeyDateFormat, CompanyCurrent.Id, CompanyCurrent.TenantId);

            if (systemSetting != null && systemSetting.Value != null)
                dateSystemFormat = systemSetting.Value;

            var model = new OrderModel();
            model.OrderCode = OrderCodeGenerate();

            model.OrderCreateDateFormat = DateTime.Now.ToString(dateSystemFormat); ;
            model.ExpectedDeliveryDate = DateTime.Now;
            model.EstimatedDeliveryStartDate = DateTime.Now;
            model.EstimatedDeliveryEndDate = DateTime.Now;

            model.SystemFormatDateTime = dateSystemFormat;

            Session["CustomerFromId"] = null;
            Session["CustomerToId"] = null;

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult Create(OrderModel model)
        {
            try
            {
                model.CustomerFromId = Session["CustomerFromId"] != null ?
                    Convert.ToInt32(Session["CustomerFromId"]) : 0;

                model.CustomerToId = Session["CustomerToId"] != null ?
                    Convert.ToInt32(Session["CustomerToId"]) : 0;

                if (ModelState.IsValid)
                {
                    var orderEntity = new Order();
                    orderEntity.OrderCode = model.OrderCode.Trim();
                    orderEntity.Remark = model.Remark;
                    orderEntity.CompanyId = CompanyCurrent.Id;
                    orderEntity.TenantId = CompanyCurrent.TenantId;
                    orderEntity.CreatedById = UserCurrent.UserId;
                    orderEntity.UpdatedById = UserCurrent.UserId;
                    orderEntity.CreatedDate = DateTime.Now;
                    orderEntity.UpdatedDate = DateTime.Now;

                    //Check duplicate data
                    //if (_orderService.CheckExistData(0, model.OrderCode.Trim(), CompanyCurrent.Id, CompanyCurrent.TenantId))
                    //{
                    //    ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS006", MessageManager.GetCaptionValueByKey("lblUserGroup")));
                    //}
                    //else
                    //{
                    //    _orderService.InsertOrder(orderEntity);

                    //    SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS003"));

                    //    //if (continueEditing)
                    //    //{
                    //    //    return RedirectToAction("Edit", new { id = orderEntity.Id });
                    //    //}
                    //    return RedirectToAction("List");
                    //}
                }
                else
                {
                    //ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS004"));
                }

                //Session["AlertSuccess"] = true;

                return Json(new { saveSuccess = true, orderCode = "ABCORD20210730AEBGM" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public virtual ActionResult Edit(string OrderCode)
        {
            try
            {
                SetActiveFunction(FunctionConst.OrderUrl);
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var order = _orderService.GetByOrderCode(OrderCode, CompanyCurrent.Id, CompanyCurrent.TenantId); //_orderService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (order == null)
                    return RedirectToAction("List");

                var model = new OrderModel();
                model = PrepareDataModel(order);

                Session["CustomerFromId"] = model.CustomerFromId;
                Session["CustomerToId"] = model.CustomerToId;

                //if (Session["AlertSuccess"] != null)
                //{
                //    SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS003"));
                //}

                //Session["AlertSuccess"] = null;

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Edit(OrderModel model)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                model.CustomerFromId = Session["CustomerFromId"] != null ?
                    Convert.ToInt32(Session["CustomerFromId"]) : 0;

                model.CustomerToId = Session["CustomerToId"] != null ?
                    Convert.ToInt32(Session["CustomerToId"]) : 0;

                var order = _orderService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (order == null)
                {
                    //No order found with the specified id
                    return RedirectToAction("List");
                }

                if (ModelState.IsValid)
                {
                    var orderUpdate = new Order();
                    orderUpdate = PrepareDataUpdate(model, order);

                    _orderService.UpdateOrder(orderUpdate);

                    return Json(new { saveSuccess = true, messageId = "MS003" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { saveSuccess = false, messageId = "MS009" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var order = _orderService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (order == null)
                    //No manufacturer found with the specified id
                    return RedirectToAction("List");

                _orderService.DeleteOrder(order);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                //SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS007"));

                return Json(new { Result = true });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                //    return AccessDeniedView();

                if (selectedIds != null)
                {
                    foreach (var id in selectedIds)
                    {
                        var order = _orderService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (order != null)
                            _orderService.DeleteOrder(order);
                        else
                            continue;
                    }
                }

                //SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS007"));

                return Json(new { Result = true });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        #endregion Create / Edit / Delete

        #region Prepare Model

        public OrderModel PrepareDataModel(Order order)
        {
            try
            {
                var systemSetting = _systemSettingService.GetByKey(SystemSettingConst.KeyDateFormat, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (systemSetting != null && systemSetting.Value != null)
                    dateSystemFormat = systemSetting.Value;

                #region Order Status

                var orderStatusName = GetOrderStatusName(order.OrderStatusId);

                #endregion Order Status

                #region Local Currency

                var localCurrencyName = GetLocalCurrency();

                #endregion Local Currency

                #region Order Shipping Status

                var orderShippingStatusName = GetOrderShippingStatusName(order.OrderShippingStatusId);

                #endregion Order Shipping Status

                #region Transporter

                var transporterName = GetTransporterName(order.TransporterId);

                #endregion Transporter

                #region Transporter

                var vehicleTypeName = GetVehicleTypeName(order.VehicleTypeId);

                #endregion Transporter

                var model = new OrderModel();

                model.Id = order.Id;
                model.OrderCode = order.OrderCode;
                model.BillOfLadingCode = order.BillOfLadingCode;
                model.Remark = order.Remark;
                model.OrderCreateDateFormat = order.OrderCreateDate.ToString(dateSystemFormat);
                model.ExpectedDeliveryDate = order.ExpectedDeliveryDate;
                model.EstimatedDeliveryStartDate = order.EstimatedDeliveryStartDate;
                model.EstimatedDeliveryEndDate = order.EstimatedDeliveryEndDate;
                model.EstimatedNumberDaysDelivery = order.EstimatedNumberDaysDelivery;
                model.OrderStatusId = order.OrderStatusId;
                model.OrderShippingStatusId = order.OrderShippingStatusId;
                model.OrderStatusName = orderStatusName;
                model.OrderShippingStatusName = orderShippingStatusName;
                model.IsBillOfExchange = order.IsBillOfExchange.GetValueOrDefault();
                model.WorkflowId = order.WorkflowId;
                model.TransporterId = order.TransporterId;
                model.LocalCurrency = localCurrencyName;
                model.PayerPostageServiceId = order.PayerPostageServiceId;
                model.VehicleTypeId = order.VehicleTypeId;
                //model.VehicleTypeName = vehicleTypeName;
                model.RouteId = order.RouteId;

                #region Customer From To

                model.CustomerFromId = order.CustomerFromId;
                model.CustomerToId = order.CustomerToId;

                var customerFrom = _customerService.GetById(order.CustomerFromId, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (customerFrom != null)
                {
                    model.CustomerFromCode = customerFrom.CustomerCode;
                    model.CustomerFromName = customerFrom.CustomerName;
                    model.CustomerFromPhone1 = customerFrom.Phone1;
                    model.CustomerFromPhone2 = customerFrom.Phone2;
                    model.CustomerFromTaxCode = customerFrom.TaxCode;
                    model.CustomerFromIdentityCardNumber = customerFrom.IdentityCardNumber;
                    model.CustomerFromFullAddress = customerFrom.FullAddress;
                }

                var customerTo = _customerService.GetById(order.CustomerToId, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (customerTo != null)
                {
                    model.CustomerToCode = customerTo.CustomerCode;
                    model.CustomerToName = customerTo.CustomerName;
                    model.CustomerToPhone1 = customerTo.Phone1;
                    model.CustomerToPhone2 = customerTo.Phone2;
                    model.CustomerToTaxCode = customerTo.TaxCode;
                    model.CustomerToIdentityCardNumber = customerTo.IdentityCardNumber;
                    model.CustomerToFullAddress = customerTo.FullAddress;
                }

                #endregion Customer From To

                #region Address From To

                model.CountryFromId = order.CountryFromId;
                model.ProvinceFromId = order.ProvinceFromId;
                model.DistrictFromId = order.DistrictFromId;
                model.WardFromId = order.WardFromId;
                model.AddressFrom = order.AddressFrom;

                model.CountryToId = order.CountryToId;
                model.ProvinceToId = order.ProvinceToId;
                model.DistrictToId = order.DistrictToId;
                model.WardToId = order.WardToId;
                model.AddressTo = order.AddressTo;

                #endregion Address From To

                #region Order Summary

                model.TotalWeight = order.TotalWeight;
                model.WeightTypeId = order.WeightTypeId;
                model.IsCollectingMoney = order.IsCollectingMoney.GetValueOrDefault();
                model.TotalCollectingMoney = order.TotalCollectingMoney;
                model.CurrencyId = order.CurrencyId;
                model.TotalOrderValue = order.TotalOrderValue;
                model.TotalService = order.TotalService;
                model.TotalPostage = order.TotalPostage;
                model.TotalTax = order.TotalTax;
                model.TotalReceivable = order.TotalReceivable;

                #endregion Order Summary

                model.SystemFormatDateTime = dateSystemFormat;
                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public Order PrepareDataUpdate(OrderModel model, Order order)
        {
            try
            {
                var orderUpdate = new Order();
                orderUpdate = order;

                orderUpdate.OrderCode = model.OrderCode;
                orderUpdate.UpdatedById = UserCurrent.UserId;
                orderUpdate.UpdatedDate = DateTime.Now;

                return orderUpdate;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Prepare Model

        #region Order Item Detail

        [HttpPost]
        public virtual ActionResult OrderDetailsByOrderId(int orderId, DataSourceRequest command)
        {
            try
            {
                //order detail

                var modelOrderItemDetails = new List<OrderItemDetailModel>();

                var orderItemDetails = _orderItemDetailService.GetByOrderItemId(orderId, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (orderItemDetails != null && orderItemDetails.Count > 0)
                {
                    var _stt = 1;
                    foreach (var orderItem in orderItemDetails)
                    {
                        #region Weight Type

                        var weightType = "";
                        if (orderItem.WeightTypeId != null)
                        {
                            var getWeightType = _weightTypeService.GetById(Convert.ToInt32(orderItem.WeightTypeId));

                            if (getWeightType != null)
                            {
                                weightType = _fixDataTranslationService.GetName(LanguageCurrent.Id, getWeightType.TranslationId);

                                if (string.IsNullOrEmpty(weightType))
                                {
                                    weightType = getWeightType.Name;
                                }
                            }
                        }

                        #endregion Weight Type

                        #region Length Type

                        var itemLength = "";
                        var itemWidth = "";
                        var itemHeight = "";

                        if (orderItem.ItemLengthLengthTypeId != null)
                        {
                            var getitemLengthType = _lengthTypeService.GetById(Convert.ToInt32(orderItem.ItemLengthLengthTypeId));

                            if (getitemLengthType != null)
                            {
                                itemLength = _fixDataTranslationService.GetName(LanguageCurrent.Id, getitemLengthType.TranslationId);

                                if (string.IsNullOrEmpty(itemLength))
                                {
                                    itemLength = getitemLengthType.Name;
                                }
                            }
                        }

                        if (orderItem.ItemWidthLengthTypeId != null)
                        {
                            var getItemWidthLengthType = _lengthTypeService.GetById(Convert.ToInt32(orderItem.ItemWidthLengthTypeId));

                            if (getItemWidthLengthType != null)
                            {
                                itemWidth = _fixDataTranslationService.GetName(LanguageCurrent.Id, getItemWidthLengthType.TranslationId);

                                if (string.IsNullOrEmpty(itemWidth))
                                {
                                    itemWidth = getItemWidthLengthType.Name;
                                }
                            }
                        }

                        if (orderItem.ItemHeightLengthTypeId != null)
                        {
                            var getItemHeightLengthType = _lengthTypeService.GetById(Convert.ToInt32(orderItem.ItemHeightLengthTypeId));

                            if (getItemHeightLengthType != null)
                            {
                                itemHeight = _fixDataTranslationService.GetName(LanguageCurrent.Id, getItemHeightLengthType.TranslationId);

                                if (string.IsNullOrEmpty(itemHeight))
                                {
                                    itemHeight = getItemHeightLengthType.Name;
                                }
                            }
                        }

                        #endregion Length Type

                        #region Item Type

                        var itemType = "";
                        if (orderItem.ItemTypeId != null)
                        {
                            var getItemType = _itemTypeService.GetById(Convert.ToInt32(orderItem.ItemTypeId));

                            if (getItemType != null)
                            {
                                itemType = _masterDataTranslationService.GetName(LanguageCurrent.Id, getItemType.TranslationId);

                                if (string.IsNullOrEmpty(itemType))
                                {
                                    itemType = getItemType.Name;
                                }
                            }
                        }

                        #endregion Item Type

                        #region Item Unit

                        var itemUnit = "";
                        if (orderItem.ItemUnitId != null)
                        {
                            var getItemUnit = _itemUnitService.GetById(Convert.ToInt32(orderItem.ItemUnitId), CompanyCurrent.Id, CompanyCurrent.TenantId);

                            if (getItemUnit != null)
                            {
                                itemUnit = _masterDataTranslationService.GetName(LanguageCurrent.Id, getItemUnit.TranslationId);

                                if (string.IsNullOrEmpty(itemType))
                                {
                                    itemUnit = getItemUnit.Name;
                                }
                            }
                        }

                        #endregion Item Unit

                        var model = new OrderItemDetailModel();

                        model.STT = _stt;
                        model.ItemName = orderItem.ItemName;
                        model.Amount = orderItem.Amount;
                        model.Weight = orderItem.Weight;
                        model.WeightType = weightType;
                        model.ItemLength = string.Format("{0} ({1})", orderItem.ItemLength, itemLength);
                        model.ItemWidth = string.Format("{0} ({1})", orderItem.ItemWidth, itemWidth);
                        model.ItemHeight = string.Format("{0} ({1})", orderItem.ItemHeight, itemHeight);
                        model.ItemType = itemType;
                        model.ItemUnit = itemUnit;

                        modelOrderItemDetails.Add(model);

                        _stt += 1;
                    }
                }
                var gridModel = new DataSourceResult
                {
                    Data = modelOrderItemDetails,
                    Total = modelOrderItemDetails.Count
                };

                return Json(gridModel);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        #endregion Order Item Detail

        #region Get Data Setup & Master

        public string GetOrderStatusName(int orderStatusId)
        {
            try
            {
                var orderStatusName = "";
                var orderStatus = _orderStatusService.GetById(orderStatusId);

                if (orderStatus != null)
                {
                    orderStatusName = _fixDataTranslationService.GetName(LanguageCurrent.Id, orderStatus.TranslationId);

                    if (string.IsNullOrEmpty(orderStatusName))
                        orderStatusName = orderStatus.Name;
                }

                return orderStatusName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return "";
            }
        }

        public string GetOrderShippingStatusName(int orderShippingStatusId)
        {
            try
            {
                var orderShippingStatusName = "";
                var orderShippingStatus = _orderShippingStatusService.GetById(orderShippingStatusId);

                if (orderShippingStatus != null)
                {
                    orderShippingStatusName = _fixDataTranslationService.GetName(LanguageCurrent.Id, orderShippingStatus.TranslationId);

                    if (string.IsNullOrEmpty(orderShippingStatusName))
                        orderShippingStatusName = orderShippingStatus.Name;
                }

                return orderShippingStatusName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetTransporterName(int transporterId)
        {
            try
            {
                var transporterName = "";
                var transporter = _transporterService.GetById(transporterId);

                if (transporter != null)
                {
                    transporterName = _masterDataTranslationService.GetName(LanguageCurrent.Id, transporter.TranslationId);

                    if (string.IsNullOrEmpty(transporterName))
                        transporterName = transporter.Name;
                }

                return transporterName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetVehicleTypeName(int vehicleTypeId)
        {
            try
            {
                var vehicleTypeName = "";
                var vehicleType = _vehicleTypeService.GetById(Convert.ToInt32(vehicleTypeId));

                if (vehicleType != null)
                {
                    vehicleTypeName = _masterDataTranslationService.GetName(LanguageCurrent.Id, vehicleType.TranslationId);

                    if (string.IsNullOrEmpty(vehicleTypeName))
                        vehicleTypeName = vehicleType.Name;
                }

                return vehicleTypeName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetCountryName(int countryId)
        {
            try
            {
                var countryName = "";
                var country = _countryService.GetById(countryId);

                if (country != null)
                {
                    countryName = _masterDataTranslationService.GetName(LanguageCurrent.Id, country.TranslationId);

                    if (string.IsNullOrEmpty(countryName))
                        countryName = country.Name;
                }

                return countryName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetProvinceName(int provinceId)
        {
            try
            {
                var provinceName = "";
                var province = _provinceService.GetById(provinceId);

                if (province != null)
                {
                    provinceName = _masterDataTranslationService.GetName(LanguageCurrent.Id, province.TranslationId);

                    if (string.IsNullOrEmpty(provinceName))
                        provinceName = province.Name;
                }

                return provinceName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetDistrictName(int districtId)
        {
            try
            {
                var districtName = "";
                var district = _districtService.GetById(districtId);

                if (district != null)
                {
                    districtName = _masterDataTranslationService.GetName(LanguageCurrent.Id, district.TranslationId);

                    if (string.IsNullOrEmpty(districtName))
                        districtName = district.Name;
                }

                return districtName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetWardName(int wardId)
        {
            try
            {
                var wardName = "";
                var ward = _wardService.GetById(wardId);

                if (ward != null)
                {
                    wardName = _masterDataTranslationService.GetName(LanguageCurrent.Id, ward.TranslationId);

                    if (string.IsNullOrEmpty(wardName))
                        wardName = ward.Name;
                }

                return wardName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetLocalCurrency()
        {
            try
            {
                var localCurrencyName = "";
                var localCurrency = _currencyService.GetLocalCurrencyByCompanyId(CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (localCurrency != null)
                {
                    localCurrencyName = _masterDataTranslationService.GetName(LanguageCurrent.Id, localCurrency.TranslationId);

                    if (string.IsNullOrEmpty(localCurrencyName))
                        localCurrencyName = localCurrency.Name;
                }

                return localCurrencyName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public string GetRouteName(int routeId)
        {
            try
            {
                var routeName = "";
                var route = _routeService.GetById(routeId);

                if (route != null)
                {
                    routeName = _masterDataTranslationService.GetName(LanguageCurrent.Id, route.TranslationId);

                    if (string.IsNullOrEmpty(routeName))
                        routeName = route.Name;
                }

                return routeName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        #endregion Get Data Setup & Master

        #region Get Customer From - To

        public string GetCustomer(int customerId)
        {
            try
            {
                var customerName = "";
                var customer = _customerService.GetById(customerId, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (customer != null)
                {
                    customerName = string.Format("{0} - {1}", customer.CustomerCode, customer.CustomerName);
                }

                return customerName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return null;
            }
        }

        #endregion Get Customer From - To

        #region Selected Customer From To

        //CustomerSelected

        [HttpPost]
        public virtual ActionResult CustomerSelected(int customerId, int typeId)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageProducts))
                //    return AccessDeniedView();

                var customer = _customerService.GetById(customerId, CompanyCurrent.Id, CompanyCurrent.TenantId);

                if (typeId == CustomerTypeConst.CustomerFrom) // CustomerFrom
                    Session["CustomerFromId"] = customer.Id;
                if (typeId == CustomerTypeConst.CustomerTo) // CustomerTo
                    Session["CustomerToId"] = customer.Id;

                return Json(new { result = customer });
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return null;
            }
        }

        #endregion Selected Customer From To

        #region Generate Order Code

        public string OrderCodeGenerate()
        {
            try
            {
                var orderCode = "";
                orderCode = string.Format("{0}{1}{2}{3}", CompanyCurrent.Code, FunctionConst.OrderGenerateCode, DateTime.Now.ToString("yyyyMMdd"), RandomCharacter.Random(8));
                return orderCode;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                return "";
            }
        }

        #endregion Generate Order Code
    }
}
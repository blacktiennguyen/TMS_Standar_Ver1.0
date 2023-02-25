using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Extend;
using TMS.Library.Commons;
using TMS.Service.Customers;
using TMS.Service.FixDataTranslations;
using TMS.Service.FIXs;
using TMS.Service.MasterDatas;
using TMS.Service.MasterDataTranslations;
using TMS.Service.Orders;
using TMS.Service.Users;
using TMS.WebAPP.Framework.Controllers;
using TMS.WebAPP.Models;
using TMS.WebAPP.Models.Order;

namespace TMS.WebAPP.Controllers
{
    public class CustomerController : TMSBaseController
    {
        #region Fields

        private readonly ICustomerService _customerService;
        private readonly IMasterDataTranslationService _masterDataTranslationService;

        #endregion Fields

        #region Constructors

        public CustomerController(ICustomerService customerService,
            IMasterDataTranslationService masterDataTranslationService)
        {
            this._customerService = customerService;
            this._masterDataTranslationService = masterDataTranslationService;
        }

        #endregion Constructors

        #region List

        // GET: ChooseCustomer (on Onder chooose customer)
        public ActionResult ChooseCustomer()
        {
            CustomerModel model = new CustomerModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult ChooseCustomer(DataSourceRequest command, CustomerModel model)
        {
            try
            {
                var customers = _customerService.SearchChooseCustomer(command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = customers.Select(x =>
                    {
                        return new CustomerModel
                        {
                            Id = x.Id,
                            CustomerCode = x.CustomerCode,
                            CustomerName = x.CustomerName,
                            Phone1 = x.Phone1,
                            Phone2 = x.Phone2,
                            TaxCode = x.TaxCode,
                            Email = x.Email,
                            IdentityCardNumber = x.IdentityCardNumber
                        };
                    }),
                    Total = customers.TotalCount
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

        #endregion List
    }
}
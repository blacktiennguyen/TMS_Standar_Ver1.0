using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Extend;
using TMS.Service;
using TMS.Service.Companys;
using TMS.Service.FunctionTranslations;
using TMS.Service.Languages;
using TMS.Shared.Const;
using TMS.WebAPP.Models;

namespace TMS.WebAPP.Controllers
{
    public class FunctionController : TMSBaseController
    {
        #region Fields

        private readonly IFunctionService _functionService;
        private readonly IFunctionTranslationService _functionTranslationService;
        private readonly ILanguageService _languageService;
        private readonly ICompanyService _companyService;

        #endregion Fields

        #region Constructors

        public FunctionController(IFunctionService functionService,
            IFunctionTranslationService functionTranslationService,
            ILanguageService languageService,
            ICompanyService companyService)
        {
            this._functionService = functionService;
            this._functionTranslationService = functionTranslationService;
            this._languageService = languageService;
            this._companyService = companyService;
        }

        #endregion Constructors

        // GET: Function
        public ActionResult FunctionNavigation(string url)
        {
            try
            {
                var model = new HomePageModel.FunctionModel();
                model.SubFunctions = PrepareFunctionModel(null, url).ToList();

                #region Get Language

                ViewBag.MainNavTitle = MessageManager.GetCaptionValueByKey("lblNavigation");

                #endregion Get Language

                #region Get Data Function

                foreach (var obj in model.SubFunctions)
                {
                    var urlFunctionLevel1 = "/" + obj.Controller + "/" + obj.Action;
                    if (urlFunctionLevel1 == url)
                    {
                        obj.IsActive = true;
                        obj.ClassActive = "active";
                    }

                    foreach (var obj2 in obj.SubFunctions)
                    {
                        var n = Session["UrlFunctionActive"] != null ?
                        Session["UrlFunctionActive"].ToString() : "";
                        var urlFunctionLevel2 = "/" + obj2.Controller + "/" + obj2.Action;
                        if (urlFunctionLevel2 == url || (Session["UrlFunctionActive"] != null &&
                            Session["UrlFunctionActive"].ToString() == urlFunctionLevel2))
                        {
                            obj2.IsActive = true;
                            obj2.ClassActive = "active";
                            obj.IsActive = true;
                            obj.ClassActive = "active";
                        }

                        //foreach (var obj3 in obj2.SubFunctions)
                        //{
                        //    var test = Session["UrlFunctionActive"] != null ? Session["UrlFunctionActive"].ToString() : "";
                        //    var urlFunctionLevel3 = "/" + obj3.Controller + "/" + obj3.Action;
                        //    if (urlFunctionLevel3 == url || (Session["UrlFunctionActive"] != null &&
                        //        Session["UrlFunctionActive"].ToString() == url))
                        //    {
                        //        obj3.IsActive = true;
                        //        obj3.ClassActive = "active";
                        //        obj2.IsActive = true;
                        //        obj2.ClassActive = "active";
                        //        obj.IsActive = true;
                        //        obj.ClassActive = "active";
                        //    }
                        //}
                    }
                }
                ViewBag.CurrentUsername = UserCurrent.UserName;

                //Session["UrlFunctionActive"] = null;

                #endregion Get Data Function

                return PartialView(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [NonAction]
        protected IList<HomePageModel.FunctionModel> PrepareFunctionModel(int? parentID, string currentUrl)
        {
            try
            {
                var result = new List<HomePageModel.FunctionModel>();

                foreach (var function in _functionService.GetAllFuntions(parentID, CompanyCurrent.Id, CompanyCurrent.TenantId))
                {
                    var nameFunctionTranslation = _functionTranslationService.GetName(LanguageCurrent.Id, function.TranslationId);

                    var functionModel = new HomePageModel.FunctionModel()
                    {
                        Id = function.Id,
                        Name = nameFunctionTranslation != null ? nameFunctionTranslation : function.Name,
                        Controller = function.Controller,
                        Action = function.Action,
                        CssIcon = function.CssIcon
                    };
                    functionModel.SubFunctions.AddRange(PrepareFunctionModel(function.Id, currentUrl));

                    result.Add(functionModel);
                }

                return result;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public ActionResult FunctionPageHeader(string url)
        {
            try
            {
                #region Ctor Modle

                var model = new HomePageModel.FunctionPageHeaderModel();
                var languageAlls = new List<LanguageModel>();
                var companyAlls = new List<CompanyModel>();

                #endregion Ctor Modle

                #region Set Language Current

                ViewBag.CurrentLanguageId = LanguageCurrent.Id;
                ViewBag.CurrentLanguageName = LanguageCurrent.Name;
                ViewBag.CurrentIconLanguage = LanguageCurrent.Icon;

                #endregion Set Language Current

                #region Set Combobox Language All

                languageAlls = GetAllLanguage();
                model.Languages = languageAlls;

                #endregion Set Combobox Language All

                #region Get Companys by user login

                //Get All Companys User Login. Is Tenant get all else current company
                var listCompanyModles = new List<CompanyModel>();

                var companys = UserCurrent.Companys;

                if (companys != null && companys.Count > 0)
                {
                    foreach (var c in companys)
                    {
                        var companyModle = new CompanyModel();
                        companyModle.Id = c.Id;
                        companyModle.Name = c.Name;

                        listCompanyModles.Add(companyModle);
                    }
                }

                model.Companys = listCompanyModles;

                #endregion Get Companys by user login

                #region Set User Info

                if (UserCurrent.UserId > 0)
                {
                    ViewBag.CurrentUserName = UserCurrent.UserName;
                    ViewBag.JobTitle = UserCurrent.JobTitle;
                    ViewBag.JoinDate = UserCurrent.CreatedDate.ToString("dd/MM/yyyy");
                }

                #endregion Set User Info

                model.CompanyCurrentId = CompanyCurrent.Id;

                return PartialView(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public List<LanguageModel> GetAllLanguage()
        {
            try
            {
                var languageModels = new List<LanguageModel>();

                var getAllLanguages = _languageService.GetAll();
                if (getAllLanguages != null && getAllLanguages.Count > 0)
                {
                    foreach (var l in getAllLanguages)
                    {
                        var languageModel = new LanguageModel();
                        languageModel.Id = l.Id;
                        languageModel.Code = l.Code;
                        languageModel.Name = l.Name;
                        languageModel.Icon = l.Icon;

                        languageModels.Add(languageModel);
                    }
                }

                return languageModels;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        #region Load Menu Child for DataSetup or Report

        // Get Show When Click Data Setup Menu From Menu Parent (Menu Left Main) // Master Menu Datasetup and Report
        public ActionResult ShowFunctionDataSetupAndReport(string url)
        {
            //Get Type DataSetup or Type Report (ex: Order Setup)
            var functionId = GetFunctionIdByUrlFunctionAction(url);

            var model = new HomePageModel.FunctionModel();
            model.SubFunctions = PrepareFunctionModel(functionId, url).ToList();

            return View(model);
        }

        // Show Menu Data Setup When Click Menu Data Setup (Menu children)
        public ActionResult FunctionDataSetupAndReport(string url)
        {
            //Get Type DataSetup or Type Report (ex: Order Setup)
            url = Session["UrlFunctionActive"].ToString();

            var functionId = GetFunctionIdByUrlFunctionAction(url);

            var model = new HomePageModel.FunctionModel();
            model.SubFunctions = PrepareFunctionModel(functionId, url).ToList();

            return View(model);
        }

        public int GetFunctionIdByUrlFunctionAction(string functionUrl)
        {
            int id = 0;

            if (functionUrl == FunctionConst.OrderDataSetupUrl)
                id = FunctionConst.OrderDataSetup;

            return id;
        }

        #endregion Load Menu Child for DataSetup or Report
    }
}
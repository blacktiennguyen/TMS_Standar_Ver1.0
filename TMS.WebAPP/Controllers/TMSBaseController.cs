using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.SessionEntity;
using TMS.Library.Commons;
using TMS.WebAPP.Framework.UI;
using TMS.Core.Extend;
using System.Text;
using System.Web.Caching;
using System.Web.Configuration;
using TMS.Service.MasterDataTranslations;

namespace TMS.WebAPP.Controllers
{
    [SessionExpireFilter]
    public class TMSBaseController : Base
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TMSBaseController()
        {
        }

        /// <summary>
        /// Display success notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void SuccessNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Success, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void ErrorNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Error, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display error notification
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        /// <param name="logException">A value indicating whether exception should be logged</param>
        protected virtual void ErrorNotification(Exception exception, bool persistForTheNextRequest = true, bool logException = true)
        {
            if (logException)
                logger.Error(logException);
            AddNotification(NotifyType.Error, exception.Message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display warning notification
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void WarningNotification(string message, bool persistForTheNextRequest = true)
        {
            AddNotification(NotifyType.Warning, message, persistForTheNextRequest);
        }

        /// <summary>
        /// Display notification
        /// </summary>
        /// <param name="type">Notification type</param>
        /// <param name="message">Message</param>
        /// <param name="persistForTheNextRequest">A value indicating whether a message should be persisted for the next request</param>
        protected virtual void AddNotification(NotifyType type, string message, bool persistForTheNextRequest)
        {
            string dataKey = string.Format("tms.notifications.{0}", type);
            if (persistForTheNextRequest)
            {
                if (TempData[dataKey] == null)
                    TempData[dataKey] = new List<string>();
                ((List<string>)TempData[dataKey]).Add(message);
            }
            else
            {
                if (ViewData[dataKey] == null)
                    ViewData[dataKey] = new List<string>();
                ((List<string>)ViewData[dataKey]).Add(message);
            }
        }

        #region Set Active Function Menu

        //Set active menu left function
        public void SetActiveFunction(string functionUrl)
        {
            Session["UrlFunctionActive"] = functionUrl;
        }

        // Set Active menu children datasetup
        public void SetActiveFunctionDataSetup(string datasetupUrl)
        {
            Session["UrlDataSetupActive"] = datasetupUrl;
        }

        // Set Active menu children report
        public void SetActiveFunctionReport(string reportUrl)
        {
        }

        #endregion Set Active Function Menu

        #region Save Or Update, Delete MasterDataTranslation

        public virtual void SaveOrUpdateMasterDataTranslation(IMasterDataTranslationService _masterDataTranslationService, string nameLL,
            Guid translationId, string tableName)
        {
            try
            {
                if (translationId != Guid.Empty && translationId != null)
                {
                    var updateMasterDataTranslation = new MasterDataTranslation();

                    using (var db = new TMSContext())
                    {
                        updateMasterDataTranslation = db.MasterDataTranslations
                            .Where(x => x.LanguageId == LanguageCurrent.Id && x.TranslationId == translationId)
                            .FirstOrDefault();
                    }

                    if (updateMasterDataTranslation != null)
                    {
                        updateMasterDataTranslation.Name = nameLL;
                        updateMasterDataTranslation.TableName = tableName;
                        updateMasterDataTranslation.UpdatedById = UserCurrent.UserId;
                        updateMasterDataTranslation.UpdatedDate = DateTime.Now;

                        _masterDataTranslationService.SaveOrUpdate(updateMasterDataTranslation);
                    }
                    else
                    {
                        var masterDataTranslation = new MasterDataTranslation();
                        masterDataTranslation.Name = nameLL;
                        masterDataTranslation.TranslationId = translationId;
                        masterDataTranslation.LanguageId = LanguageCurrent.Id;
                        masterDataTranslation.TableName = tableName;
                        masterDataTranslation.CreatedById = UserCurrent.UserId;
                        masterDataTranslation.CreatedDate = DateTime.Now;
                        masterDataTranslation.UpdatedById = null;
                        masterDataTranslation.UpdatedDate = null;

                        _masterDataTranslationService.SaveOrUpdate(masterDataTranslation);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return;
            }
        }

        public virtual void DeleteMasterDataTranslation(IMasterDataTranslationService _masterDataTranslationService, Guid translationId)
        {
            try
            {
                if (translationId != Guid.Empty && translationId != null)
                {
                    var updateMasterDataTranslation = new MasterDataTranslation();

                    using (var db = new TMSContext())
                    {
                        updateMasterDataTranslation = db.MasterDataTranslations
                            .Where(x => x.LanguageId == LanguageCurrent.Id && x.TranslationId == translationId)
                            .FirstOrDefault();
                    }

                    if (updateMasterDataTranslation != null)
                    {
                        _masterDataTranslationService.Delete(updateMasterDataTranslation);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return;
            }
        }

        #endregion Save Or Update, Delete MasterDataTranslation
    }

    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            SessionUserCurrent userCurrent = (SessionUserCurrent)(filterContext.HttpContext.Session[SessionConstant.UserCurrent]);
            if (userCurrent.UserId == 0)
            {
                string returnurl = filterContext.HttpContext.Request.RawUrl.ToString();
                returnurl = System.Web.HttpUtility.UrlEncode(returnurl);

                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.RequestContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.Clear();
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Account/Login?returnUrl=" + returnurl);
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class Base : Controller
    {
        public ISessionWrapper SessionWrapper { get; set; }

        public bool isLogin { get; set; }

        public Base()
        {
            if (SessionWrapper == null)
            {
                SessionWrapper = new HttpContextSessionWrapper();
            }
            try
            {
                #region Set Session User Current

                SessionUserCurrent sessionUserCurrent;
                sessionUserCurrent = SessionWrapper.GetFromSession<SessionUserCurrent>(SessionConstant.UserCurrent);
                if (sessionUserCurrent == null)
                    sessionUserCurrent = new SessionUserCurrent();

                SessionWrapper.SetInSession(SessionConstant.UserCurrent, sessionUserCurrent);

                #endregion Set Session User Current

                #region Set Session Language Current

                SessionLanguageCurrent sessionLanguageCurrent;
                sessionLanguageCurrent = SessionWrapper.GetFromSession<SessionLanguageCurrent>(SessionConstant.LanguageCurrent);
                if (sessionLanguageCurrent == null)
                    sessionLanguageCurrent = new SessionLanguageCurrent();

                SessionWrapper.SetInSession(SessionConstant.LanguageCurrent, sessionLanguageCurrent);

                #endregion Set Session Language Current

                #region Set Session Company Current

                SessionCompanyCurrent sessionCompanyCurrent;
                sessionCompanyCurrent = SessionWrapper.GetFromSession<SessionCompanyCurrent>(SessionConstant.CompanyCurrent);
                if (sessionLanguageCurrent == null)
                    sessionCompanyCurrent = new SessionCompanyCurrent();

                SessionWrapper.SetInSession(SessionConstant.CompanyCurrent, sessionCompanyCurrent);

                #endregion Set Session Company Current
            }
            catch
            {
            }

            SessionUserCurrent customerLogin = SessionWrapper.GetFromSession<SessionUserCurrent>(SessionConstant.UserCurrent);
            if (customerLogin != null && customerLogin.UserId > 0)
            {
                isLogin = true;

                #region Get User from DataBase Set To Session (for Case change db but user not logout and login)

                var userDb = new User();

                using (var db = new TMSContext())
                {
                    userDb = db.Users
                        .Where(x => x.IsActive == true && x.TenantId == customerLogin.TenantId &&
                        x.CompanyId == customerLogin.CompanyId)
                        .FirstOrDefault();
                }

                if (userDb != null)
                {
                    // Set Session User get from Database Realtime
                    customerLogin.UserId = userDb.Id;
                    customerLogin.UserName = userDb.UserName;
                    customerLogin.CompanyId = userDb.CompanyId;
                    customerLogin.TenantId = userDb.TenantId;
                    customerLogin.IsTenant = userDb.IsTenant;
                    customerLogin.FullName = userDb.FullName;
                    customerLogin.JobTitle = userDb.JobTitle;
                    customerLogin.AvartarUrl = userDb.AvartarUrl;
                    customerLogin.CreatedDate = userDb.CreatedDate;
                }

                #endregion Get User from DataBase Set To Session (for Case change db but user not logout and login)

                #region Set User Current Login get from Session

                #region Get ListCompany from Db of User Login

                var getAllCompanys = new List<Company>();
                var getCompanyById = new Company();

                #region GetAllCompanys By User

                using (var db = new TMSContext())
                {
                    getAllCompanys = db.Companys
                        .Where(x => x.IsActive == true && x.TenantId == customerLogin.TenantId)
                        .ToList();
                }

                #endregion GetAllCompanys By User

                #region Get Company By User Login

                using (var db = new TMSContext())
                {
                    getCompanyById = db.Companys
                        .Where(x => x.Id == customerLogin.CompanyId && x.IsActive == true && x.TenantId == customerLogin.TenantId)
                        .FirstOrDefault();
                }

                #endregion Get Company By User Login

                var userCompanys = new List<UserCompany>();

                if (customerLogin.IsTenant)// is user tenant get all company of tenant
                {
                    if (getAllCompanys != null && getAllCompanys.Count > 0)
                    {
                        foreach (var c in getAllCompanys)
                        {
                            var userCompany = new UserCompany();
                            userCompany.Id = c.Id;
                            userCompany.Name = c.Name;
                            userCompany.Code = c.Code;
                            userCompany.Email = c.Email;
                            userCompany.Fax = c.Fax;
                            userCompany.Phone = c.Phone;
                            userCompany.UserNumber = c.UserNumber;
                            userCompany.TenantId = c.TenantId;
                            userCompany.Address = c.Address;
                            userCompany.IsActive = c.IsActive;

                            userCompanys.Add(userCompany);
                        }
                    }
                }
                else
                {
                    if (getCompanyById != null)
                    {
                        var userCompany = new UserCompany();
                        userCompany.Id = getCompanyById.Id;
                        userCompany.Name = getCompanyById.Name;
                        userCompany.Code = getCompanyById.Code;
                        userCompany.Email = getCompanyById.Email;
                        userCompany.Fax = getCompanyById.Fax;
                        userCompany.Phone = getCompanyById.Phone;
                        userCompany.UserNumber = getCompanyById.UserNumber;
                        userCompany.TenantId = getCompanyById.TenantId;
                        userCompany.Address = getCompanyById.Address;
                        userCompany.IsActive = getCompanyById.IsActive;

                        userCompanys.Add(userCompany);
                    }
                }

                #endregion Get ListCompany from Db of User Login

                UserCurrent.UserId = customerLogin.UserId;
                UserCurrent.UserName = customerLogin.UserName;
                UserCurrent.FullName = customerLogin.FullName;
                UserCurrent.IsTenant = customerLogin.IsTenant;
                UserCurrent.AvartarUrl = customerLogin.AvartarUrl;
                UserCurrent.JobTitle = customerLogin.JobTitle;
                UserCurrent.CreatedDate = customerLogin.CreatedDate;
                UserCurrent.CompanyId = customerLogin.CompanyId;
                UserCurrent.TenantId = customerLogin.TenantId;
                UserCurrent.Companys = userCompanys;

                #endregion Set User Current Login get from Session

                #region Set Company Current get from Session

                SessionCompanyCurrent sessionCompanyCurrent =
                    SessionWrapper.GetFromSession<SessionCompanyCurrent>(SessionConstant.CompanyCurrent);

                if (sessionCompanyCurrent != null && sessionCompanyCurrent.Id > 0)
                {
                    CompanyCurrent.Id = sessionCompanyCurrent.Id;
                    CompanyCurrent.Code = sessionCompanyCurrent.Code;
                    CompanyCurrent.Name = sessionCompanyCurrent.Name;
                    CompanyCurrent.IsActive = sessionCompanyCurrent.IsActive;
                    CompanyCurrent.Fax = sessionCompanyCurrent.Fax;
                    CompanyCurrent.Phone = sessionCompanyCurrent.Phone;
                    CompanyCurrent.TenantId = sessionCompanyCurrent.TenantId;
                    CompanyCurrent.UserNumber = sessionCompanyCurrent.UserNumber;
                }

                #endregion Set Company Current get from Session
            }
            else
            {
                isLogin = false;
            }

            #region Set Lanuage Current from Session

            //Set Langugage Current from Session
            SessionLanguageCurrent getLanguageCurrent = SessionWrapper.GetFromSession<SessionLanguageCurrent>(SessionConstant.LanguageCurrent);
            if (getLanguageCurrent != null && getLanguageCurrent.LanguageId > 0)
            {
                //LanguageCurrent = new LanguageCurrent();
                LanguageCurrent.Id = getLanguageCurrent.LanguageId;
                LanguageCurrent.Name = getLanguageCurrent.LanguageName;
                LanguageCurrent.Icon = getLanguageCurrent.IconLanguage;
            }
            else
            {
                using (var db = new TMSContext())
                {
                    var languageDefault = db.Languages
                        .Where(x => x.IsActive == true && x.IsDefault)
                        .FirstOrDefault();

                    if (languageDefault != null)
                    {
                        LanguageCurrent.Id = languageDefault.Id;
                        LanguageCurrent.Name = languageDefault.Name;
                        LanguageCurrent.Icon = languageDefault.Icon;
                    }
                }
            }

            #endregion Set Lanuage Current from Session
        }

        public bool IsUserLogin()
        {
            return isLogin;
        }
    }

    public class HttpContextSessionWrapper : ISessionWrapper
    {
        public T GetFromSession<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public void SetInSession(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }

    public class HttpPostOrRedirectAttribute : ActionFilterAttribute
    {
        public string RedirectAction { get; set; }
        public string RedirectController { get; set; }
        public string[] ParametersToPassWithRedirect { get; set; }

        public HttpPostOrRedirectAttribute(string redirectAction)
            : this(redirectAction, null, new string[] { })
        {
        }

        public HttpPostOrRedirectAttribute(string redirectAction, string[] parametersToPassWithRedirect)
            : this(redirectAction, null, parametersToPassWithRedirect)
        {
        }

        public HttpPostOrRedirectAttribute(string redirectAction, string redirectController, string[] parametersToPassWithRedirect)
        {
            RedirectAction = redirectAction;
            RedirectController = redirectController;
            ParametersToPassWithRedirect = parametersToPassWithRedirect;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.HttpMethod == "POST")
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                string redirectUrl = GetRedirectUrl(filterContext.RequestContext);
                filterContext.Controller.TempData["Warning"] = "Your action could not be completed as your"
                    + " session had expired.  Please try again.";
                filterContext.Result = new RedirectResult(redirectUrl);
            }
        }

        public string GetRedirectUrl(RequestContext context)
        {
            RouteValueDictionary routeValues = new RouteValueDictionary();
            foreach (string parameter in ParametersToPassWithRedirect)
            {
                if (context.RouteData.Values.ContainsKey(parameter))
                    routeValues.Add(parameter, context.RouteData.Values[parameter]);
            }
            string controller = RedirectController
                ?? context.RouteData.Values["controller"].ToString();
            UrlHelper urlHelper = new UrlHelper(context);
            return urlHelper.Action(RedirectAction, controller, routeValues);
        }
    }
}

public class MessageManager
{
    private static double CacheCaptionExpireMinute = Convert.ToInt64(WebConfigurationManager.AppSettings["CacheCaptionExpireMinute"]);
    private static double CacheMessageExpireMinute = Convert.ToInt64(WebConfigurationManager.AppSettings["CacheMessageExpireMinute"]);

    private static global::System.Resources.ResourceManager _ResourceManager;

    public static global::System.Resources.ResourceManager ResourceManager
    {
        get
        {
            return _ResourceManager;
        }
        set
        {
            _ResourceManager = value;
        }
    }

    public static String GetCaptionValueByKey(String key)
    {
        try
        {
            var caption = "";

            if (System.Web.HttpContext.Current.Cache[key + LanguageCurrent.Id] == null)
            {
                using (var db = new TMSContext())
                {
                    var captionValue = db.CaptionTranslations
                        .Where(x => x.LanguageId == LanguageCurrent.Id && x.CaptionKey == key)
                        .FirstOrDefault();

                    caption = captionValue?.CaptionValue;
                }

                System.Web.HttpContext.Current.Cache.Add(key + LanguageCurrent.Id, caption, null, DateTime.Now.AddMinutes(CacheCaptionExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
            else
            {
                caption = System.Web.HttpContext.Current.Cache[key + LanguageCurrent.Id] as string;
            }

            return caption;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    public static string GetMessageInfoByMessageCode(string msgCode)
    {
        try
        {
            var resultMes = "";

            if (System.Web.HttpContext.Current.Cache[msgCode + LanguageCurrent.Id] == null)
            {
                using (var db = new TMSContext())
                {
                    var mesInfo = db.MessageInfoTranslations
                        .Where(x => x.LanguageId == LanguageCurrent.Id && x.MsgCode == msgCode)
                        .FirstOrDefault();

                    resultMes = mesInfo != null ? mesInfo.MsgCode + ": " + mesInfo.MsgContent : "";
                }

                System.Web.HttpContext.Current.Cache.Add(msgCode + LanguageCurrent.Id, resultMes, null, DateTime.Now.AddMinutes(CacheMessageExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
            else
            {
                resultMes = System.Web.HttpContext.Current.Cache[msgCode + LanguageCurrent.Id] as string;
            }

            return resultMes;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    public static string GetMessageInfoByMessageCode(string msgCode, string captionFieldName)
    {
        try
        {
            var resultMes = "";

            if (System.Web.HttpContext.Current.Cache[msgCode + LanguageCurrent.Id + captionFieldName] == null)
            {
                using (var db = new TMSContext())
                {
                    var captionField = GetCaptionValueByKey(captionFieldName);

                    var messInfo = db.MessageInfoTranslations
                        .Where(x => x.LanguageId == LanguageCurrent.Id && x.MsgCode == msgCode)
                        .FirstOrDefault();

                    var contentMsg = messInfo != null ? string.Format(messInfo.MsgContent, captionField) : "";

                    resultMes = messInfo != null ? messInfo.MsgCode + ": " + contentMsg : "";
                }

                System.Web.HttpContext.Current.Cache.Add(msgCode + LanguageCurrent.Id + captionFieldName, resultMes, null, DateTime.Now.AddMinutes(CacheMessageExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
            else
            {
                resultMes = System.Web.HttpContext.Current.Cache[msgCode + LanguageCurrent.Id + captionFieldName] as string;
            }

            return resultMes;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    public static string GetMessageInfoByMessageCode(string msgCode, string captionFieldName1, string captionFieldName2)
    {
        try
        {
            var resultMes = "";

            if (System.Web.HttpContext.Current.Cache[msgCode + LanguageCurrent.Id + captionFieldName1 + captionFieldName2] == null)
            {
                using (var db = new TMSContext())
                {
                    var _captionFieldName1 = GetCaptionValueByKey(captionFieldName1);
                    var _captionFieldName2 = GetCaptionValueByKey(captionFieldName2);

                    var messInfo = db.MessageInfoTranslations
                        .Where(x => x.LanguageId == LanguageCurrent.Id && x.MsgCode == msgCode)
                        .FirstOrDefault();

                    var contentMsg = messInfo != null ? string.Format(messInfo.MsgContent, _captionFieldName1, _captionFieldName2) : "";

                    resultMes = messInfo != null ? messInfo.MsgCode + ": " + contentMsg : "";
                }

                System.Web.HttpContext.Current.Cache.Add(msgCode + LanguageCurrent.Id + captionFieldName1 + captionFieldName2, resultMes, null, DateTime.Now.AddMinutes(CacheMessageExpireMinute), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.AboveNormal, null);
            }
            else
            {
                resultMes = System.Web.HttpContext.Current.Cache[msgCode + LanguageCurrent.Id + captionFieldName1 + captionFieldName2] as string;
            }

            return resultMes;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    //public static string GetMessageInfoByMessageCode(string msgCode, string fieldName)
    //{
    //    try
    //    {
    //        using (var db = new TMSContext())
    //        {
    //            var resultMes = "";

    //            var mesInfo = db.MessageInfoTranslations
    //                .Where(x => x.LanguageId == LanguageCurrent.Id && x.MsgCode == msgCode)
    //                .FirstOrDefault();

    //            if(mesInfo != null)
    //            {
    //                resultMes = mesInfo.MsgContent + ": " + string.Format(mesInfo.MsgContent, fieldName);
    //            }

    //            return resultMes;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        return string.Empty;
    //        throw;
    //    }
    //}
    public static String GetStringAppConfig(String key)
    {
        return ConfigurationManager.AppSettings[key];
    }

    public static String GetMessage2(String key, params Object[] args)
    {
        Object[] args2 = new Object[args.Length];
        String msg = GetMessage(key);
        for (int i = 0; i < args.Length; i++)
        {
            args2[i] = args[i];
        }
        String str = String.Format(msg, args2);
        return str;
    }

    public static String GetMessage(String key, params Object[] args)
    {
        Object[] args2 = new Object[args.Length];
        String msg = GetMessage(key);
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].GetType() == typeof(String))
                args2[i] = GetMessage((String)args[i]);
            else
                args2[i] = args[i];
            if (args2[i] == null || args2[i].ToString() == String.Empty)
                args2[i] = args[i];
        }
        String str = String.Format(msg, args2);
        return str;
    }

    public static String GetMessage(String key, Object arg1)
    {
        Object[] args = new Object[] { arg1 };
        return GetMessage(key, args);
    }

    public static String GetMessage(String key, Object arg1, Object arg2)
    {
        Object[] args = new Object[] { arg1, arg2 };
        return GetMessage(key, args);
    }

    public static String GetMessage(String key, Object arg1, Object arg2, Object arg3)
    {
        Object[] args = new Object[] { arg1, arg2, arg3 };
        return GetMessage(key, args);
    }
}
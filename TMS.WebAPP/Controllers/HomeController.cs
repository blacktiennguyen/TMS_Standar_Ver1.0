using System;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.SessionEntity;
using TMS.Library.Commons;
using TMS.Service.Companys;
using TMS.Service.Languages;

namespace TMS.WebAPP.Controllers
{
    public class HomeController : TMSBaseController
    {
        #region Fields

        private readonly ILanguageService _languageService;

        private readonly ICompanyService _companyService;

        #endregion Fields

        #region Constructors

        public HomeController(ILanguageService languageService, ICompanyService companyService)
        {
            this._languageService = languageService;
            this._companyService = companyService;
        }

        #endregion Constructors

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SelectedLanguage(int languageId)
        {
            try
            {
                var language = _languageService.GetById(languageId);
                var sessionLanguageCurrent = new SessionLanguageCurrent();

                if (language != null)
                {
                    sessionLanguageCurrent.LanguageId = language.Id;
                    sessionLanguageCurrent.LanguageName = language.Name;
                    sessionLanguageCurrent.IconLanguage = language.Icon;
                }

                SessionWrapper.SetInSession(SessionConstant.LanguageCurrent, sessionLanguageCurrent);

                return Json(new { mess = "", data = language }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [HttpPost]
        public JsonResult SelectedCompany(int companyId)
        {
            try
            {
                var company = _companyService.GetById(companyId, CompanyCurrent.TenantId);
                var sessionCompanyCurrent = new SessionCompanyCurrent();

                if (company != null)
                {
                    sessionCompanyCurrent.Id = company.Id;
                    sessionCompanyCurrent.Code = company.Code;
                    sessionCompanyCurrent.Name = company.Name;
                    sessionCompanyCurrent.IsActive = company.IsActive;
                    sessionCompanyCurrent.Fax = company.Fax;
                    sessionCompanyCurrent.Phone = company.Phone;
                    sessionCompanyCurrent.TenantId = company.TenantId;
                    sessionCompanyCurrent.UserNumber = company.UserNumber;
                }

                SessionWrapper.SetInSession(SessionConstant.CompanyCurrent, sessionCompanyCurrent);

                return Json(new { mess = "", data = company }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Extend;
using TMS.Core.SessionEntity;
using TMS.Library.Commons;
using TMS.Service.Companys;
using TMS.Service.Languages;
using TMS.Service.Users;
using TMS.WebAPP.Models;
using TMS.WebAPP.Models.AccountModel;

namespace TMS.WebAPP.Controllers
{
    public class AccountController : Base
    {
        #region Fields

        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;
        private readonly ILanguageService _languageService;

        #endregion Fields

        #region Constructors

        public AccountController(IUserService userService,
            ICompanyService companyService,
            ILanguageService languageService)
        {
            this._userService = userService;
            this._companyService = companyService;
            this._languageService = languageService;
        }

        #endregion Constructors

        //GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            try
            {
                var loginViewModel = new LoginViewModel();

                loginViewModel.Languages = GetAllLanguage();

                //if (Convert.ToInt32(Session["atempt"]) >= 5)
                //{
                //    ModelState.AddModelError("", "Bạn đã đăng nhập sai quá 5 lần ,vui lòng đăng nhập lại sau 30 phút hoặc Xóa session");
                //    Session.Timeout = 30;

                //}

                if (isLogin == false)
                {
                    ViewBag.ReturnUrl = returnUrl;
                    HttpCookie objCookie = new HttpCookie("hpsURL", returnUrl);

                    ViewBag.LanguageName = LanguageCurrent.Name;
                    ViewBag.Icon = LanguageCurrent.Icon;
                }
                else
                {
                    if (Request.Cookies["hpsURL"] != null)
                    {
                        returnUrl = Request.Cookies["hpsURL"].Value;
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                returnUrl = System.Web.HttpUtility.UrlDecode(returnUrl);
                if (ModelState.IsValid)
                {
                    if (Convert.ToInt32(Session["atempt"]) >= 5)
                    {
                        ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("MS001"));
                        Session.Timeout = 30;
                    }
                    else
                    {
                        var getUser = _userService.LoginUser(model.UserName, model.Password);

                        if (getUser == null) //login k thanh cong
                        {
                            ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("VR001"));
                            Session["atempt"] = Convert.ToInt32(Session["atempt"]) + 1;
                            Session.Timeout = 2;

                            if (Convert.ToInt32(Session["atempt"]) >= 4)
                            {
                                if (ModelState.ContainsKey(""))
                                    ModelState[""].Errors.Clear();
                                ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("MS002"));
                                Session.Timeout = 2;
                            }
                            if (Convert.ToInt32(Session["atempt"]) >= 5)
                            {
                                if (ModelState.ContainsKey(""))
                                    ModelState[""].Errors.Clear();
                                ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("MS001"));
                                Session.Timeout = 30;
                            }
                        }
                        else
                        {
                            
                            //if (userE != null)
                            {
                                HttpCookie cookie_username = new HttpCookie("cookiescmUsername");
                                cookie_username.Value = getUser.UserName;
                                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie_username);

                                #region Set UserCurrent from session

                                #region Set ListCompany of User Login

                                var getAllCompanys = _companyService.GetAll(getUser.TenantId);
                                var getCompanyById = _companyService.GetById(getUser.CompanyId, getUser.TenantId);

                                var userCompanys = new List<UserCompany>();

                                if (getUser != null)
                                {
                                    if (getUser.IsTenant)// is user tenant get all company of tenant
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
                                }

                                #endregion Set ListCompany of User Login

                                SessionUserCurrent sessionUserCurrent = new SessionUserCurrent();
                                sessionUserCurrent.UserId = getUser.Id;
                                sessionUserCurrent.UserName = getUser.UserName;
                                sessionUserCurrent.AvartarUrl = getUser.AvartarUrl;
                                sessionUserCurrent.FullName = getUser.FullName;
                                sessionUserCurrent.JobTitle = getUser.JobTitle;
                                sessionUserCurrent.CreatedDate = getUser.CreatedDate;
                                sessionUserCurrent.IsTenant = getUser.IsTenant;
                                sessionUserCurrent.CompanyId = getUser.CompanyId;
                                sessionUserCurrent.TenantId = getUser.TenantId;
                                sessionUserCurrent.Companys = userCompanys;

                                SessionWrapper.SetInSession(SessionConstant.UserCurrent, sessionUserCurrent);

                                #endregion Set UserCurrent from session

                                #region Set Company Current Session of User Login (Company selected combobox where view data from change combobox)

                                var getCompany = _companyService.GetById(getUser.CompanyId, getUser.TenantId);

                                if (getCompany != null)
                                {
                                    SessionCompanyCurrent sessionCompanyCurrent = new SessionCompanyCurrent();
                                    sessionCompanyCurrent.Id = getCompany.Id;
                                    sessionCompanyCurrent.Code = getCompany.Code;
                                    sessionCompanyCurrent.Name = getCompany.Name;
                                    sessionCompanyCurrent.TenantId = getCompany.TenantId;
                                    sessionCompanyCurrent.UserNumber = getCompany.UserNumber;
                                    sessionCompanyCurrent.IsActive = getCompany.IsActive;
                                    sessionCompanyCurrent.Fax = getCompany.Fax;
                                    sessionCompanyCurrent.Phone = getCompany.Phone;
                                    sessionCompanyCurrent.Email = getCompany.Email;
                                    sessionCompanyCurrent.Address = getCompany.Address;

                                    SessionWrapper.SetInSession(SessionConstant.CompanyCurrent, sessionCompanyCurrent);
                                }

                                #endregion Set Company Current Session of User Login (Company selected combobox where view data from change combobox)

                                if (!String.IsNullOrEmpty(returnUrl))
                                {
                                    return Redirect(returnUrl);
                                }
                                else
                                {
                                    if (Request.Cookies["hpsURL"] != null)
                                    {
                                        returnUrl = Request.Cookies["hpsURL"].Value;
                                        return Redirect(returnUrl);
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }
                            //else
                            //{
                            //    ModelState.AddModelError("", "Đăng nhập thất bại: Tài khoản " + model.UserName + " chưa tồn tại trên hệ thống");
                            //}
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(model.UserName))
                    {
                        ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("VR004"));
                    }
                    if (string.IsNullOrWhiteSpace(model.Password))
                    {
                        ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("VR005"));
                    }
                }

                #region Language

                // Set get all Language and set curent language selected
                model.Languages = GetAllLanguage();
                ViewBag.LanguageName = LanguageCurrent.Name;
                ViewBag.Icon = LanguageCurrent.Icon;

                #endregion Language

                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public ActionResult LogOff()
        {
            SessionWrapper.SetInSession(SessionConstant.UserCurrent, null);
            return RedirectToAction("Index", "Home");
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
                ModelState.AddModelError("", MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }
    }
}
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMS.Core;
using TMS.Core.Domains;
using TMS.Core.Extend;
using TMS.Service.UserGroups;
using TMS.Service.Users;
using TMS.Shared.Const;
using TMS.WebAPP.Framework.Controllers;
using TMS.WebAPP.Models;

namespace TMS.WebAPP.Controllers
{
    public class UserGroupController : TMSBaseController
    {
        #region Fields

        private readonly IGroupService _groupService;

        private readonly IUserService _userService;

        #endregion Fields

        #region Constructors

        public UserGroupController(IGroupService groupService, IUserService userService)
        {
            this._groupService = groupService;
            this._userService = userService;
        }

        #endregion Constructors

        #region List

        // GET: GroupUser
        public ActionResult GroupList()
        {
            GroupModel model = new GroupModel();

            return View(model);
        }

        [HttpPost]
        public ActionResult GroupList(DataSourceRequest command, GroupModel model)
        {
            try
            {
                var groups = _groupService.SearchGroup(model.Name, command.Page - 1, command.PageSize, CompanyCurrent.Id, CompanyCurrent.TenantId);

                var gridModel = new DataSourceResult
                {
                    Data = groups.Select(x =>
                    {
                        var createdByUser = _userService.GetById(x.CreatedById);

                        return new GroupModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            IsActive = x.IsActive,
                            Remark = x.Remark,
                            CreatedBy = createdByUser != null ? createdByUser.UserName : "",
                            CreateDate = x.CreatedDate != null ?
                            LanguageCurrent.Id == LanguageIdConst.LanguageENG ? x.CreatedDate.ToString("MM/dd/yyyy") : x.CreatedDate.ToString("dd/MM/yyyy") : string.Empty
                        };
                    }),
                    Total = groups.TotalCount
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

        #region Create / Edit / Delete

        public virtual ActionResult Create()
        {
            var model = new GroupModel();

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Create(GroupModel model, bool continueEditing)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var groupEntity = new Group();
                    groupEntity.Name = model.Name.Trim();
                    groupEntity.IsActive = model.IsActive;
                    groupEntity.Remark = model.Remark;
                    groupEntity.CompanyId = CompanyCurrent.Id;
                    groupEntity.TenantId = CompanyCurrent.TenantId;
                    groupEntity.CreatedById = UserCurrent.UserId;
                    groupEntity.UpdatedById = UserCurrent.UserId;
                    groupEntity.CreatedDate = DateTime.Now;
                    groupEntity.UpdatedDate = DateTime.Now;

                    //Check duplicate data
                    if (_groupService.CheckExistData(0, model.Name.Trim(), CompanyCurrent.Id, CompanyCurrent.TenantId))
                    {
                        ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS006", MessageManager.GetCaptionValueByKey("lblUserGroup")));
                    }
                    else
                    {
                        _groupService.InsertGroup(groupEntity);

                        SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS003"));

                        if (continueEditing)
                        {
                            return RedirectToAction("Edit", new { id = groupEntity.Id });
                        }
                        return RedirectToAction("GroupList");
                    }
                }
                else
                {
                    ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS004"));
                }

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        public virtual ActionResult Edit(int id)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var group = _groupService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (group == null)
                    //No manufacturer found with the specified id
                    return RedirectToAction("GroupList");

                var model = new GroupModel();
                model = PrepareDataModel(group);

                return View(model);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual ActionResult Edit(GroupModel model, bool continueEditing)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var group = _groupService.GetById(model.Id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (group == null)
                    //No manufacturer found with the specified id
                    return RedirectToAction("GroupList");

                if (ModelState.IsValid)
                {
                    group.Name = model.Name;
                    group.Remark = model.Remark;
                    group.IsActive = model.IsActive;
                    group.UpdatedDate = DateTime.Now;
                    group.UpdatedById = UserCurrent.UserId;

                    //Check duplicate data
                    if (_groupService.CheckExistData(group.Id, model.Name.Trim(), CompanyCurrent.Id, CompanyCurrent.TenantId))
                    {
                        ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS006", MessageManager.GetCaptionValueByKey("lblUserGroup")));
                    }
                    else
                    {
                        _groupService.UpdateGroup(group);

                        SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS003"));

                        if (continueEditing)
                        {
                            return RedirectToAction("Edit", new { id = group.Id });
                        }
                        return RedirectToAction("GroupList");
                    }
                }

                model = PrepareDataModel(group);

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
        public virtual ActionResult Delete(int id)
        {
            try
            {
                //if (!_permissionService.Authorize(StandardPermissionProvider.ManageManufacturers))
                //    return AccessDeniedView();

                var group = _groupService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);
                if (group == null)
                    //No manufacturer found with the specified id
                    return RedirectToAction("GroupList");

                _groupService.DeleteGroup(group);

                ////activity log
                //_customerActivityService.InsertActivity("DeleteManufacturer", _localizationService.GetResource("ActivityLog.DeleteManufacturer"), manufacturer.Name);

                SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS007"));
                return RedirectToAction("GroupList");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        [HttpPost]
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
                        var group = _groupService.GetById(id, CompanyCurrent.Id, CompanyCurrent.TenantId);

                        if (group != null)
                            _groupService.DeleteGroup(group);
                        else
                            continue;
                    }
                }

                SuccessNotification(MessageManager.GetMessageInfoByMessageCode("MS007"));

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

        #region Prepare GroupModel

        public GroupModel PrepareDataModel(Group group)
        {
            try
            {
                var model = new GroupModel();
                model.Id = group.Id;
                model.Name = group.Name;
                model.IsActive = group.IsActive;
                model.Remark = group.Remark;

                return model;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                ErrorNotification(MessageManager.GetMessageInfoByMessageCode("MS005"));
                throw;
            }
        }

        #endregion Prepare GroupModel
    }
}
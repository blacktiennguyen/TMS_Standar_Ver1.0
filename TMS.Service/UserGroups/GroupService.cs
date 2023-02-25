using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.UserGroups
{
    public class GroupService : IGroupService
    {
        public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Fields

        private readonly IRepository<Group> _groupRepository;

        #endregion Fields

        #region Ctor

        public GroupService(IRepository<Group> groupRepository)
        {
            this._groupRepository = groupRepository;
        }

        #endregion Ctor

        public Group GetById(int Id, int companyId, int tenantId)
        {
            var group = new Group();
            try
            {
                using (var db = new TMSContext())
                {
                    group = db.Groups
                        .Where(x => x.Id == Id && x.CompanyId == companyId && x.TenantId == tenantId)
                        .FirstOrDefault();

                    return group;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public bool CheckExistData(int id = 0, string name = "", int companyId = 0, int tenantId = 0)
        {
            var isExist = false;
            try
            {
                using (var db = new TMSContext())
                {
                    isExist = db.Groups
                        .Where(x => x.Id != id && x.Name == name && x.CompanyId == companyId && x.TenantId == tenantId)
                        .Any();

                    return isExist;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return false;
            }
        }

        public IPagedList<Group> SearchGroup(string groupName, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    var query = db.Groups
                        .Where(x => x.CompanyId == companyId && x.TenantId == tenantId)
                        .ToList();

                    if (!String.IsNullOrEmpty(groupName))
                        query = query.Where(x => x.Name != null && !String.IsNullOrEmpty(x.Name) && x.Name.Contains(groupName))
                                .ToList();

                    query = query.OrderByDescending(x => x.CreatedDate).ToList();

                    return new PagedList<Group>(query, pageIndex, pageSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        #region Insert / Update / Delete

        public void InsertGroup(Group group)
        {
            try
            {
                _groupRepository.Insert(group);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void UpdateGroup(Group group)
        {
            try
            {
                using (var db = new TMSContext())
                {
                    db.Entry(group).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        public void DeleteGroup(Group group)
        {
            try
            {
                _groupRepository.Delete(group);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                throw;
            }
        }

        #endregion Insert / Update / Delete
    }
}
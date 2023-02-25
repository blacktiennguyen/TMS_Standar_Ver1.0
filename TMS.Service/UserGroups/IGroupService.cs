using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core;
using TMS.Core.Domains;

namespace TMS.Service.UserGroups
{
    public partial interface IGroupService
    {
        Group GetById(int Id, int companyId, int tenantId);

        bool CheckExistData(int id = 0, string name = "", int companyId = 0, int tenantId = 0);

        IPagedList<Group> SearchGroup(string groupName, int pageIndex = 0, int pageSize = int.MaxValue, int companyId = 0, int tenantId = 0);

        void InsertGroup(Group group);

        void UpdateGroup(Group group);

        void DeleteGroup(Group group);
    }
}
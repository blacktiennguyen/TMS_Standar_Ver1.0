using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.WebAPP.Models
{
    public class UserGroupModel
    {
        public UserGroupModel()
        {
            Group = new GroupModel();
        }

        public GroupModel Group { get; set; }
    }
}
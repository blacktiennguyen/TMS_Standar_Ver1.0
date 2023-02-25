using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TMS.WebAPP.Framework.Mvc;

namespace TMS.WebAPP.Models
{
    public class GroupModel : BaseTMSEntityModel
    {
        [Required]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string Remark { get; set; }

        public string CreatedBy { get; set; }

        public string CreateDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMS.WebAPP.Models.MasterDataModel
{
    public class ItemUnitModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NameLL { get; set; }

        public Guid TranslationId { get; set; }

        public string Remark { get; set; }

        public int CompanyId { get; set; }

        public int TenantId { get; set; }
    }
}
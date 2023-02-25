using System;
using System.Collections.Generic;

namespace TMS.Core.Domains.MasterDatas
{
    public class Province : BaseEntity
    {
        public int CountryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Remark { get; set; }

        public Guid? TranslationId { get; set; }
    }
}
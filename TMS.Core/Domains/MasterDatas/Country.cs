using System;
using System.Collections.Generic;

namespace TMS.Core.Domains.MasterDatas
{
    public class Country : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool AllowsBilling { get; set; }

        public bool AllowsShipping { get; set; }

        public string TwoLetterIsoCode { get; set; }
        public string ThreeLetterIsoCode { get; set; }

        public int NumericIsoCode { get; set; }

        public bool IsDefault { get; set; }

        public Guid? TranslationId { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Core.Domains.FIXs
{
    public class WeightType : BaseEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public Guid TranslationId { get; set; }
    }
}
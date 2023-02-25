using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.FIXs;

namespace TMS.Core.Mappings.FIXs
{
    public class LengthTypeMapping : EntityTypeConfiguration<LengthType>
    {
        public LengthTypeMapping()
        {
            this.ToTable("FIX_LengthTypes");
            this.HasKey(a => a.Id);
        }
    }
}
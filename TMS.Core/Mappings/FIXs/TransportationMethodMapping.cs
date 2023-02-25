using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.FIXs;

namespace TMS.Core.Mappings.FIXs
{
    public class TransportationMethodMapping : EntityTypeConfiguration<TransportationMethod>
    {
        public TransportationMethodMapping()
        {
            this.ToTable("FIX_TransportationMethods");
            this.HasKey(a => a.Id);
        }
    }
}
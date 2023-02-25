using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.FIXs;

namespace TMS.Core.Mappings.FIXs
{
    public class PayerPostageServiceMapping : EntityTypeConfiguration<PayerPostageService>
    {
        public PayerPostageServiceMapping()
        {
            this.ToTable("FIX_PayerPostageServices");
            this.HasKey(a => a.Id);
        }
    }
}
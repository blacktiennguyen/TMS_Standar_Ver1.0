using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.FIXs;

namespace TMS.Core.Mappings.FIXs
{
    public class OrderShippingStatusMapping : EntityTypeConfiguration<OrderShippingStatus>
    {
        public OrderShippingStatusMapping()
        {
            this.ToTable("FIX_OrderShippingStatus");
            this.HasKey(a => a.Id);
        }
    }
}
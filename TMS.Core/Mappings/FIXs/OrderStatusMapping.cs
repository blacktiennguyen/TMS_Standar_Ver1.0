using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.FIXs;

namespace TMS.Core.Mappings.FIXs
{
    public class OrderStatusMapping : EntityTypeConfiguration<OrderStatus>
    {
        public OrderStatusMapping()
        {
            this.ToTable("FIX_OrderStatus");
            this.HasKey(a => a.Id);
        }
    }
}
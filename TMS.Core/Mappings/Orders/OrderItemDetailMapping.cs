using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class OrderItemDetailMapping : EntityTypeConfiguration<OrderItemDetail>
    {
        public OrderItemDetailMapping()
        {
            this.ToTable("ORD_OrderItemDetails");
            this.HasKey(a => a.Id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class OrderMapping : EntityTypeConfiguration<Order>
    {
        public OrderMapping()
        {
            this.ToTable("ORD_Orders");
            this.HasKey(a => a.Id);
        }
    }
}
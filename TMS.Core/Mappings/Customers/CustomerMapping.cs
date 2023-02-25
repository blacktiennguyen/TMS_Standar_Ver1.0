using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class CustomerMapping : EntityTypeConfiguration<Customer>
    {
        public CustomerMapping()
        {
            this.ToTable("CUS_Customers");
            this.HasKey(a => a.Id);
        }
    }
}
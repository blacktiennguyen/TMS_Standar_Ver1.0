using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class CompanyMapping : EntityTypeConfiguration<Company>
    {
        public CompanyMapping()
        {
            this.ToTable("MultiTenant_Companys");
            this.HasKey(a => a.Id);
        }
    }
}
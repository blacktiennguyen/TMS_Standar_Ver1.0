using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class FunctionMapping : EntityTypeConfiguration<Function>
    {
        public FunctionMapping()
        {
            this.ToTable("SYS_Functions");
            this.HasKey(a => a.Id);
        }
    }
}
using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class FunctionTranslationMapping : EntityTypeConfiguration<FunctionTranslation>
    {
        public FunctionTranslationMapping()
        {
            this.ToTable("TRANS_FunctionTranslations");
            this.HasKey(a => a.Id);
        }
    }
}
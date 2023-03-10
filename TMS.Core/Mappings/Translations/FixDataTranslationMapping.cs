using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class FixDataTranslationMapping : EntityTypeConfiguration<FixDataTranslation>
    {
        public FixDataTranslationMapping()
        {
            this.ToTable("Localization_FixDataTranslations");
            this.HasKey(a => a.Id);
        }
    }
}
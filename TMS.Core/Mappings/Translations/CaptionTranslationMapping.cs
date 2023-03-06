using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class CaptionTranslationMapping : EntityTypeConfiguration<CaptionTranslation>
    {
        public CaptionTranslationMapping()
        {
            this.ToTable("Localization_CaptionTranslations");
            this.HasKey(a => a.Id);
        }
    }
}
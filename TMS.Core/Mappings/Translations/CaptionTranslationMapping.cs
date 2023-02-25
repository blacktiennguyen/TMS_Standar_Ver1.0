using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class CaptionTranslationMapping : EntityTypeConfiguration<CaptionTranslation>
    {
        public CaptionTranslationMapping()
        {
            this.ToTable("TRANS_CaptionTranslations");
            this.HasKey(a => a.Id);
        }
    }
}
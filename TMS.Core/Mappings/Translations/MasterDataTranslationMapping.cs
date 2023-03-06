using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class MasterDataTranslationMapping : EntityTypeConfiguration<MasterDataTranslation>
    {
        public MasterDataTranslationMapping()
        {
            this.ToTable("Localization_MasterDataTranslations");
            this.HasKey(a => a.Id);
        }
    }
}
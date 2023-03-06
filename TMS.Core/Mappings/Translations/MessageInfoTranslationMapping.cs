using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class MessageInfoTranslationMapping : EntityTypeConfiguration<MessageInfoTranslation>
    {
        public MessageInfoTranslationMapping()
        {
            this.ToTable("Localization_MassageInfoTranslations");
            this.HasKey(a => a.Id);
        }
    }
}
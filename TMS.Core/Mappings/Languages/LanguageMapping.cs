using System.Data.Entity.ModelConfiguration;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class LanguageMapping : EntityTypeConfiguration<Language>
    {
        public LanguageMapping()
        {
            this.ToTable("SYS_Languages");
            this.HasKey(a => a.Id);
        }
    }
}
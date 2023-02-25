using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains;

namespace TMS.Core.Mappings.SystemSettings
{
    public class SystemSettingMapping : EntityTypeConfiguration<SystemSetting>
    {
        public SystemSettingMapping()
        {
            this.ToTable("SYS_SystemSetting");
            this.HasKey(a => a.Id);
        }
    }
}
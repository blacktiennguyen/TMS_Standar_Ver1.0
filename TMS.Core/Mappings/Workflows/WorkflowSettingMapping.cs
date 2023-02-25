using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.Workflows;

namespace TMS.Core.Mappings.Workflows
{
    public class WorkflowSettingMapping : EntityTypeConfiguration<WorkflowSetting>
    {
        public WorkflowSettingMapping()
        {
            this.ToTable("SYS_WorkflowSettings");
            this.HasKey(a => a.Id);
        }
    }
}
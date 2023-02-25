using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains;

namespace TMS.Core.Mappings
{
    public class GroupMapping : EntityTypeConfiguration<Group>
    {
        public GroupMapping()
        {
            this.ToTable("SYS_Groups");
            this.HasKey(a => a.Id);
        }
    }
}
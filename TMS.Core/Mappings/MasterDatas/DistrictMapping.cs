using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Core.Mappings.MasterDatas
{
    public class DistrictMapping : EntityTypeConfiguration<District>
    {
        public DistrictMapping()
        {
            this.ToTable("MasterData_Districts");
            this.HasKey(a => a.Id);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Core.Mappings.MasterDatas
{
    public class ProvinceMapping : EntityTypeConfiguration<Province>
    {
        public ProvinceMapping()
        {
            this.ToTable("MasterData_Provinces");
            this.HasKey(a => a.Id);
        }
    }
}
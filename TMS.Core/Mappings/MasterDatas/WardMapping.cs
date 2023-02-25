using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMS.Core.Domains.MasterDatas;

namespace TMS.Core.Mappings.MasterDatas
{
    public class WardMapping : EntityTypeConfiguration<Ward>
    {
        public WardMapping()
        {
            this.ToTable("MasterData_Wards");
            this.HasKey(a => a.Id);
        }
    }
}